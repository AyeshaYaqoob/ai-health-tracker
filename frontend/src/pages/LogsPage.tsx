import { useState } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import {
  createSymptomLog, createMoodLog, createSleepLog, createMealLog,
  getSymptomLogs, getMoodLogs, getSleepLogs, getMealLogs
} from '../api/logs'
import toast from 'react-hot-toast'
import { Activity, Smile, Moon, Utensils, Plus, Clock, AlertCircle } from 'lucide-react'

type LogTab = 'symptom' | 'mood' | 'sleep' | 'meal'
type ViewMode = 'add' | 'history'

export default function LogsPage() {
  const [activeTab, setActiveTab] = useState<LogTab>('symptom')
  const [viewMode, setViewMode] = useState<ViewMode>('add')
  const queryClient = useQueryClient()
  const today = new Date().toISOString().split('T')[0]
  const sevenDaysAgo = new Date(Date.now() - 7 * 24 * 60 * 60 * 1000).toISOString().split('T')[0]

  // Forms
  const [symptomForm, setSymptomForm] = useState({ symptomName: '', severity: 5, notes: '', logDate: today })
  const [moodForm, setMoodForm] = useState({ moodScore: 5, notes: '', logDate: today })
  const [sleepForm, setSleepForm] = useState({ hoursSlept: 7, qualityScore: 7, bedTime: '23:00:00', wakeTime: '06:00:00', logDate: today })
  const [mealForm, setMealForm] = useState({ mealType: 'Breakfast', description: '', logDate: today })

  // History queries
  const { data: symptomHistory } = useQuery({ queryKey: ['symptoms', sevenDaysAgo, today], queryFn: () => getSymptomLogs(sevenDaysAgo, today), enabled: viewMode === 'history' })
  const { data: moodHistory } = useQuery({ queryKey: ['moods', sevenDaysAgo, today], queryFn: () => getMoodLogs(sevenDaysAgo, today), enabled: viewMode === 'history' })
  const { data: sleepHistory } = useQuery({ queryKey: ['sleep', sevenDaysAgo, today], queryFn: () => getSleepLogs(sevenDaysAgo, today), enabled: viewMode === 'history' })
  const { data: mealHistory } = useQuery({ queryKey: ['meals', sevenDaysAgo, today], queryFn: () => getMealLogs(sevenDaysAgo, today), enabled: viewMode === 'history' })

  const symptomMutation = useMutation({
    mutationFn: createSymptomLog,
    onSuccess: () => {
      toast.success('Symptom logged! ✅')
      queryClient.invalidateQueries({ queryKey: ['symptoms'] })
      setSymptomForm({ symptomName: '', severity: 5, notes: '', logDate: today })
    },
    onError: () => toast.error('Failed to log symptom')
  })

  const moodMutation = useMutation({
    mutationFn: createMoodLog,
    onSuccess: () => {
      toast.success('Mood logged! 😊')
      queryClient.invalidateQueries({ queryKey: ['moods'] })
      setMoodForm({ moodScore: 5, notes: '', logDate: today })
    },
    onError: () => toast.error('Failed to log mood')
  })

  const sleepMutation = useMutation({
    mutationFn: createSleepLog,
    onSuccess: () => {
      toast.success('Sleep logged! 🌙')
      queryClient.invalidateQueries({ queryKey: ['sleep'] })
      setSleepForm({ hoursSlept: 7, qualityScore: 7, bedTime: '23:00:00', wakeTime: '06:00:00', logDate: today })
    },
    onError: () => toast.error('Failed to log sleep')
  })

  const mealMutation = useMutation({
    mutationFn: createMealLog,
    onSuccess: () => {
      toast.success('Meal logged! 🥗')
      setMealForm({ mealType: 'Breakfast', description: '', logDate: today })
    },
    onError: () => toast.error('Failed to log meal')
  })

  const tabs = [
    { id: 'symptom' as LogTab, label: 'Symptom', icon: Activity, color: 'text-rose-500', activeBg: 'bg-rose-500' },
    { id: 'mood' as LogTab, label: 'Mood', icon: Smile, color: 'text-yellow-500', activeBg: 'bg-yellow-500' },
    { id: 'sleep' as LogTab, label: 'Sleep', icon: Moon, color: 'text-indigo-500', activeBg: 'bg-indigo-600' },
    { id: 'meal' as LogTab, label: 'Meal', icon: Utensils, color: 'text-green-500', activeBg: 'bg-green-600' },
  ]

  const currentHistory = {
    symptom: symptomHistory,
    mood: moodHistory,
    sleep: sleepHistory,
    meal: mealHistory,
  }[activeTab]

  const renderHistory = () => {
    if (!currentHistory || currentHistory.length === 0) {
      return (
        <div className="py-12 text-center">
          <Clock size={36} className="mx-auto text-gray-200 mb-3" />
          <p className="text-gray-400">No {activeTab} logs in the last 7 days</p>
        </div>
      )
    }

    return (
      <div className="space-y-3">
        {activeTab === 'symptom' && (symptomHistory ?? []).map(s => (
          <div key={s.id} className="flex items-center justify-between p-4 rounded-xl border border-gray-100 hover:bg-gray-50 transition-colors">
            <div className="flex items-center gap-3">
              <div className={`w-9 h-9 rounded-xl flex items-center justify-center ${
                s.severity >= 7 ? 'bg-red-100' : s.severity >= 4 ? 'bg-amber-100' : 'bg-emerald-100'
              }`}>
                <AlertCircle size={16} className={s.severity >= 7 ? 'text-red-500' : s.severity >= 4 ? 'text-amber-500' : 'text-emerald-500'} />
              </div>
              <div>
                <p className="font-semibold text-gray-800 text-sm">{s.symptomName}</p>
                <p className="text-xs text-gray-400">{s.logDate}</p>
              </div>
            </div>
            <span className={`health-badge ${s.severity >= 7 ? 'bg-red-100 text-red-700' : s.severity >= 4 ? 'bg-amber-100 text-amber-700' : 'bg-emerald-100 text-emerald-700'}`}>
              {s.severity}/10
            </span>
          </div>
        ))}

        {activeTab === 'mood' && (moodHistory ?? []).map(m => (
          <div key={m.id} className="flex items-center justify-between p-4 rounded-xl border border-gray-100 hover:bg-gray-50 transition-colors">
            <div className="flex items-center gap-3">
              <div className="w-9 h-9 rounded-xl bg-amber-100 flex items-center justify-center">
                <Smile size={16} className="text-amber-500" />
              </div>
              <div>
                <p className="font-semibold text-gray-800 text-sm">Mood: {m.moodScore}/10</p>
                <p className="text-xs text-gray-400">{m.logDate}{m.notes ? ` • ${m.notes}` : ''}</p>
              </div>
            </div>
            <span className="health-badge bg-amber-100 text-amber-700">{m.moodScore >= 7 ? '😊' : m.moodScore >= 5 ? '😐' : '😞'} {m.moodScore}/10</span>
          </div>
        ))}

        {activeTab === 'sleep' && (sleepHistory ?? []).map(s => (
          <div key={s.id} className="flex items-center justify-between p-4 rounded-xl border border-gray-100 hover:bg-gray-50 transition-colors">
            <div className="flex items-center gap-3">
              <div className="w-9 h-9 rounded-xl bg-indigo-100 flex items-center justify-center">
                <Moon size={16} className="text-indigo-500" />
              </div>
              <div>
                <p className="font-semibold text-gray-800 text-sm">{s.hoursSlept}h sleep · Quality {s.qualityScore}/10</p>
                <p className="text-xs text-gray-400">{s.logDate} · {s.bedTime?.slice(0, 5)} → {s.wakeTime?.slice(0, 5)}</p>
              </div>
            </div>
            <span className={`health-badge ${s.hoursSlept >= 7 ? 'bg-indigo-100 text-indigo-700' : s.hoursSlept >= 6 ? 'bg-amber-100 text-amber-700' : 'bg-red-100 text-red-700'}`}>
              {s.hoursSlept}hrs
            </span>
          </div>
        ))}

        {activeTab === 'meal' && (mealHistory ?? []).map(m => (
          <div key={m.id} className="flex items-center justify-between p-4 rounded-xl border border-gray-100 hover:bg-gray-50 transition-colors">
            <div className="flex items-center gap-3">
              <div className="w-9 h-9 rounded-xl bg-emerald-100 flex items-center justify-center">
                <Utensils size={16} className="text-emerald-500" />
              </div>
              <div>
                <p className="font-semibold text-gray-800 text-sm">{m.mealType}</p>
                <p className="text-xs text-gray-400">{m.logDate} · {m.description}</p>
              </div>
            </div>
            <span className="health-badge bg-emerald-100 text-emerald-700">{m.mealType}</span>
          </div>
        ))}
      </div>
    )
  }

  return (
    <div className="p-8">
      <div className="mb-8">
        <h1 className="text-3xl font-bold text-gray-900">Daily Logs</h1>
        <p className="text-gray-500 mt-1">Track and review your health data</p>
      </div>

      {/* View mode toggle */}
      <div className="flex gap-2 mb-6">
        <button
          onClick={() => setViewMode('add')}
          className={`flex items-center gap-2 px-5 py-2.5 rounded-xl font-medium transition-all text-sm ${
            viewMode === 'add'
              ? 'text-white shadow-md'
              : 'bg-white text-gray-600 hover:bg-gray-50 border border-gray-200'
          }`}
          style={viewMode === 'add' ? { background: 'linear-gradient(135deg, #6366f1, #8b5cf6)', boxShadow: '0 4px 15px rgba(99,102,241,0.35)' } : {}}
        >
          <Plus size={16} />
          Add Log
        </button>
        <button
          onClick={() => setViewMode('history')}
          className={`flex items-center gap-2 px-5 py-2.5 rounded-xl font-medium transition-all text-sm ${
            viewMode === 'history'
              ? 'text-white shadow-md'
              : 'bg-white text-gray-600 hover:bg-gray-50 border border-gray-200'
          }`}
          style={viewMode === 'history' ? { background: 'linear-gradient(135deg, #6366f1, #8b5cf6)', boxShadow: '0 4px 15px rgba(99,102,241,0.35)' } : {}}
        >
          <Clock size={16} />
          View History
        </button>
      </div>

      {/* Category Tabs */}
      <div className="flex gap-2 mb-6 flex-wrap">
        {tabs.map(tab => {
          const Icon = tab.icon
          const isActive = activeTab === tab.id
          return (
            <button
              key={tab.id}
              onClick={() => setActiveTab(tab.id)}
              className={`flex items-center gap-2 px-5 py-2.5 rounded-xl font-medium transition-all text-sm ${
                isActive
                  ? `${tab.activeBg} text-white shadow-sm`
                  : 'bg-white text-gray-600 hover:bg-gray-50 border border-gray-200'
              }`}
            >
              <Icon size={17} />
              {tab.label}
            </button>
          )
        })}
      </div>

      {/* Form / History Panel */}
      <div className={viewMode === 'add' ? 'bg-white rounded-2xl shadow-sm border border-gray-100 p-6 max-w-lg' : 'chart-card p-6 max-w-2xl'}>

        {viewMode === 'history' ? (
          <>
            <h3 className="font-bold text-gray-800 text-lg mb-5">
              {tabs.find(t => t.id === activeTab)?.label} History — Last 7 Days
            </h3>
            {renderHistory()}
          </>
        ) : (
          <>
            {/* Symptom Form */}
            {activeTab === 'symptom' && (
              <form onSubmit={(e) => { e.preventDefault(); symptomMutation.mutate(symptomForm) }} className="space-y-4">
                <h3 className="font-semibold text-gray-800 text-lg">Log a Symptom</h3>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Symptom Name</label>
                  <input
                    type="text"
                    value={symptomForm.symptomName}
                    onChange={(e) => setSymptomForm({...symptomForm, symptomName: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-rose-400 focus:border-transparent transition-all"
                    placeholder="e.g. Headache, Fatigue, Back Pain"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Severity: <span className="text-rose-600 font-bold">{symptomForm.severity}/10</span>
                  </label>
                  <input
                    type="range" min="1" max="10"
                    value={symptomForm.severity}
                    onChange={(e) => setSymptomForm({...symptomForm, severity: Number(e.target.value)})}
                    className="w-full accent-rose-500"
                  />
                  <div className="flex justify-between text-xs text-gray-400 mt-1">
                    <span>Mild</span><span>Severe</span>
                  </div>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Notes (optional)</label>
                  <textarea
                    value={symptomForm.notes}
                    onChange={(e) => setSymptomForm({...symptomForm, notes: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-rose-400 h-20 resize-none transition-all"
                    placeholder="Any additional details..."
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Date</label>
                  <input
                    type="date"
                    value={symptomForm.logDate}
                    onChange={(e) => setSymptomForm({...symptomForm, logDate: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-rose-400 transition-all"
                  />
                </div>
                <button
                  type="submit"
                  disabled={symptomMutation.isPending}
                  className="w-full bg-rose-500 hover:bg-rose-600 text-white font-semibold py-3 rounded-xl transition-all disabled:opacity-50 shadow-sm hover:shadow-md"
                >
                  {symptomMutation.isPending ? 'Logging...' : 'Log Symptom'}
                </button>
              </form>
            )}

            {/* Mood Form */}
            {activeTab === 'mood' && (
              <form onSubmit={(e) => { e.preventDefault(); moodMutation.mutate(moodForm) }} className="space-y-4">
                <h3 className="font-semibold text-gray-800 text-lg">Log Your Mood</h3>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Mood Score: <span className="text-yellow-500 font-bold">{moodForm.moodScore}/10</span>
                  </label>
                  <div className="text-3xl text-center py-2">
                    {moodForm.moodScore >= 8 ? '😄' : moodForm.moodScore >= 6 ? '😊' : moodForm.moodScore >= 4 ? '😐' : moodForm.moodScore >= 2 ? '😕' : '😞'}
                  </div>
                  <input
                    type="range" min="1" max="10"
                    value={moodForm.moodScore}
                    onChange={(e) => setMoodForm({...moodForm, moodScore: Number(e.target.value)})}
                    className="w-full accent-yellow-500"
                  />
                  <div className="flex justify-between text-xs text-gray-400 mt-1">
                    <span>😞 Bad</span><span>😄 Great</span>
                  </div>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Notes (optional)</label>
                  <textarea
                    value={moodForm.notes}
                    onChange={(e) => setMoodForm({...moodForm, notes: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-yellow-400 h-20 resize-none transition-all"
                    placeholder="How are you feeling today?"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Date</label>
                  <input
                    type="date"
                    value={moodForm.logDate}
                    onChange={(e) => setMoodForm({...moodForm, logDate: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-yellow-400 transition-all"
                  />
                </div>
                <button
                  type="submit"
                  disabled={moodMutation.isPending}
                  className="w-full bg-yellow-500 hover:bg-yellow-600 text-white font-semibold py-3 rounded-xl transition-all disabled:opacity-50 shadow-sm hover:shadow-md"
                >
                  {moodMutation.isPending ? 'Logging...' : 'Log Mood'}
                </button>
              </form>
            )}

            {/* Sleep Form */}
            {activeTab === 'sleep' && (
              <form onSubmit={(e) => { e.preventDefault(); sleepMutation.mutate(sleepForm) }} className="space-y-4">
                <h3 className="font-semibold text-gray-800 text-lg">Log Your Sleep</h3>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Hours Slept: <span className="text-indigo-600 font-bold">{sleepForm.hoursSlept}hrs</span>
                  </label>
                  <input
                    type="range" min="1" max="12" step="0.5"
                    value={sleepForm.hoursSlept}
                    onChange={(e) => setSleepForm({...sleepForm, hoursSlept: Number(e.target.value)})}
                    className="w-full accent-indigo-600"
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">
                    Sleep Quality: <span className="text-indigo-600 font-bold">{sleepForm.qualityScore}/10</span>
                  </label>
                  <input
                    type="range" min="1" max="10"
                    value={sleepForm.qualityScore}
                    onChange={(e) => setSleepForm({...sleepForm, qualityScore: Number(e.target.value)})}
                    className="w-full accent-indigo-600"
                  />
                </div>
                <div className="grid grid-cols-2 gap-3">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">Bed Time</label>
                    <input
                      type="time"
                      value={sleepForm.bedTime.slice(0, 5)}
                      onChange={(e) => setSleepForm({...sleepForm, bedTime: e.target.value + ':00'})}
                      className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-400 transition-all"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">Wake Time</label>
                    <input
                      type="time"
                      value={sleepForm.wakeTime.slice(0, 5)}
                      onChange={(e) => setSleepForm({...sleepForm, wakeTime: e.target.value + ':00'})}
                      className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-400 transition-all"
                    />
                  </div>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Date</label>
                  <input
                    type="date"
                    value={sleepForm.logDate}
                    onChange={(e) => setSleepForm({...sleepForm, logDate: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-400 transition-all"
                  />
                </div>
                <button
                  type="submit"
                  disabled={sleepMutation.isPending}
                  className="w-full bg-indigo-600 hover:bg-indigo-700 text-white font-semibold py-3 rounded-xl transition-all disabled:opacity-50 shadow-sm hover:shadow-md"
                >
                  {sleepMutation.isPending ? 'Logging...' : 'Log Sleep'}
                </button>
              </form>
            )}

            {/* Meal Form */}
            {activeTab === 'meal' && (
              <form onSubmit={(e) => { e.preventDefault(); mealMutation.mutate(mealForm) }} className="space-y-4">
                <h3 className="font-semibold text-gray-800 text-lg">Log a Meal</h3>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Meal Type</label>
                  <select
                    value={mealForm.mealType}
                    onChange={(e) => setMealForm({...mealForm, mealType: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-emerald-400 transition-all"
                  >
                    <option>Breakfast</option>
                    <option>Lunch</option>
                    <option>Dinner</option>
                    <option>Snack</option>
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">What did you eat?</label>
                  <textarea
                    value={mealForm.description}
                    onChange={(e) => setMealForm({...mealForm, description: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-emerald-400 h-24 resize-none transition-all"
                    placeholder="e.g. Oats with banana and honey, green tea"
                    required
                  />
                </div>
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Date</label>
                  <input
                    type="date"
                    value={mealForm.logDate}
                    onChange={(e) => setMealForm({...mealForm, logDate: e.target.value})}
                    className="w-full px-4 py-2.5 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-emerald-400 transition-all"
                  />
                </div>
                <button
                  type="submit"
                  disabled={mealMutation.isPending}
                  className="w-full bg-emerald-600 hover:bg-emerald-700 text-white font-semibold py-3 rounded-xl transition-all disabled:opacity-50 shadow-sm hover:shadow-md"
                >
                  {mealMutation.isPending ? 'Logging...' : 'Log Meal'}
                </button>
              </form>
            )}
          </>
        )}
      </div>
    </div>
  )
}