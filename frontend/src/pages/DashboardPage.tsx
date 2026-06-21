import { useQuery } from '@tanstack/react-query'
import { getSymptomLogs, getMoodLogs, getSleepLogs, exportCsv, exportPdf } from '../api/logs'
import {
  XAxis, YAxis, CartesianGrid, Tooltip,
  ResponsiveContainer, BarChart, Bar, Area, AreaChart
} from 'recharts'
import {
  Activity, Moon, Smile, AlertCircle, TrendingUp, TrendingDown,
  Zap, Download, FileText, Flame
} from 'lucide-react'
import toast from 'react-hot-toast'
import { useState } from 'react'

export default function DashboardPage() {
  const today = new Date()
  const thirtyDaysAgo = new Date(today.getTime() - 30 * 24 * 60 * 60 * 1000)
  const from = thirtyDaysAgo.toISOString().split('T')[0]
  const to = today.toISOString().split('T')[0]
  const [exporting, setExporting] = useState<'csv' | 'pdf' | null>(null)

  const { data: symptoms } = useQuery({ queryKey: ['symptoms', from, to], queryFn: () => getSymptomLogs(from, to) })
  const { data: moods } = useQuery({ queryKey: ['moods', from, to], queryFn: () => getMoodLogs(from, to) })
  const { data: sleep } = useQuery({ queryKey: ['sleep', from, to], queryFn: () => getSleepLogs(from, to) })

  const avgMood = moods?.length
    ? (moods.reduce((sum, m) => sum + m.moodScore, 0) / moods.length) : 0
  const avgMoodStr = moods?.length ? avgMood.toFixed(1) : 'N/A'

  const avgSleep = sleep?.length
    ? (sleep.reduce((sum, s) => sum + s.hoursSlept, 0) / sleep.length) : 0
  const avgSleepStr = sleep?.length ? avgSleep.toFixed(1) : 'N/A'

  // Calculate streak — consecutive days with any log
  const logDates = new Set([
    ...(symptoms?.map(s => s.logDate) ?? []),
    ...(moods?.map(m => m.logDate) ?? []),
    ...(sleep?.map(s => s.logDate) ?? []),
  ])
  let streak = 0
  const checkDate = new Date()
  while (logDates.has(checkDate.toISOString().split('T')[0])) {
    streak++
    checkDate.setDate(checkDate.getDate() - 1)
  }

  // Dynamic trend label
  const recentMoods = moods?.slice(0, 5).map(m => m.moodScore) ?? []
  const moodTrending = recentMoods.length >= 2
    ? recentMoods[0] >= recentMoods[recentMoods.length - 1] ? 'up' : 'down'
    : 'neutral'

  const recentSleep = sleep?.slice(0, 5).map(s => s.hoursSlept) ?? []
  const sleepTrending = recentSleep.length >= 2
    ? recentSleep[0] >= recentSleep[recentSleep.length - 1] ? 'up' : 'down'
    : 'neutral'

  const moodChartData = moods?.slice().reverse().map(m => ({
    date: m.logDate.slice(5),
    mood: m.moodScore
  })) || []

  const sleepChartData = sleep?.slice().reverse().map(s => ({
    date: s.logDate.slice(5),
    hours: s.hoursSlept
  })) || []

  const totalLogs = (symptoms?.length || 0) + (moods?.length || 0) + (sleep?.length || 0)

  const handleExportCsv = () => {
    setExporting('csv')
    toast.success('Preparing CSV download...')
    try {
      exportCsv(from, to)
    } catch {
      toast.error('Export failed')
    }
    setTimeout(() => setExporting(null), 2000)
  }

  const handleExportPdf = () => {
    setExporting('pdf')
    toast.success('Preparing PDF report...')
    try {
      exportPdf(from, to)
    } catch {
      toast.error('Export failed')
    }
    setTimeout(() => setExporting(null), 2000)
  }

  return (
    <div className="p-8">
      {/* Header */}
      <div className="mb-8 flex items-center justify-between flex-wrap gap-4">
        <div>
          <div className="flex items-center gap-2 mb-1">
            <Zap size={16} className="text-amber-500" />
            <span className="text-sm font-semibold text-amber-600 uppercase tracking-wider">Live Dashboard</span>
          </div>
          <h1 className="text-3xl font-bold text-gray-900">Health Overview</h1>
          <p className="text-gray-500 mt-1">Your last 30 days at a glance</p>
        </div>
        <div className="flex items-center gap-3 flex-wrap">
          {/* Export buttons */}
          <button
            onClick={handleExportCsv}
            disabled={exporting === 'csv'}
            className="flex items-center gap-2 px-4 py-2 rounded-xl border border-gray-200 bg-white text-gray-600 hover:bg-gray-50 hover:border-indigo-200 transition-all text-sm font-medium"
          >
            <Download size={15} />
            {exporting === 'csv' ? 'Preparing...' : 'Export CSV'}
          </button>
          <button
            onClick={handleExportPdf}
            disabled={exporting === 'pdf'}
            className="flex items-center gap-2 px-4 py-2 rounded-xl text-white text-sm font-medium transition-all"
            style={{ background: 'linear-gradient(135deg, #6366f1, #8b5cf6)', boxShadow: '0 4px 15px rgba(99,102,241,0.35)' }}
          >
            <FileText size={15} />
            {exporting === 'pdf' ? 'Preparing...' : 'Export PDF'}
          </button>
          <div className="text-right hidden sm:block">
            <p className="text-sm text-gray-400">Today</p>
            <p className="font-semibold text-gray-700">{new Date().toLocaleDateString('en-US', { weekday: 'long', month: 'long', day: 'numeric' })}</p>
          </div>
        </div>
      </div>

      {/* Stat Cards */}
      <div className="grid grid-cols-2 lg:grid-cols-4 gap-5 mb-8">
        {/* Mood */}
        <div className="stat-card-amber rounded-2xl p-6 text-white relative overflow-hidden">
          <div className="absolute top-0 right-0 w-24 h-24 bg-white opacity-5 rounded-full -translate-y-1/2 translate-x-1/2" />
          <div className="flex items-center justify-between mb-4">
            <span className="text-amber-100 text-sm font-medium">Avg Mood</span>
            <div className="w-9 h-9 bg-white bg-opacity-20 rounded-xl flex items-center justify-center">
              <Smile size={18} className="text-white" />
            </div>
          </div>
          <p className="text-4xl font-bold">{avgMoodStr}<span className="text-xl text-amber-200 font-normal">/10</span></p>
          <p className="text-amber-200 text-xs mt-2 flex items-center gap-1">
            {moodTrending === 'up' ? <TrendingUp size={12} /> : moodTrending === 'down' ? <TrendingDown size={12} /> : <Activity size={12} />}
            {moodTrending === 'up' ? 'Trending positive' : moodTrending === 'down' ? 'Slight dip' : 'Stable'}
          </p>
        </div>

        {/* Sleep */}
        <div className="stat-card-violet rounded-2xl p-6 text-white relative overflow-hidden">
          <div className="absolute top-0 right-0 w-24 h-24 bg-white opacity-5 rounded-full -translate-y-1/2 translate-x-1/2" />
          <div className="flex items-center justify-between mb-4">
            <span className="text-purple-100 text-sm font-medium">Avg Sleep</span>
            <div className="w-9 h-9 bg-white bg-opacity-20 rounded-xl flex items-center justify-center">
              <Moon size={18} className="text-white" />
            </div>
          </div>
          <p className="text-4xl font-bold">{avgSleepStr}<span className="text-xl text-purple-200 font-normal">hrs</span></p>
          <p className="text-purple-200 text-xs mt-2 flex items-center gap-1">
            {sleepTrending === 'up' ? <TrendingUp size={12} /> : sleepTrending === 'down' ? <TrendingDown size={12} /> : <Activity size={12} />}
            {avgSleep >= 7 ? 'Above average' : avgSleep >= 6 ? 'Adequate' : 'Below target'}
          </p>
        </div>

        {/* Symptoms */}
        <div className="stat-card-rose rounded-2xl p-6 text-white relative overflow-hidden">
          <div className="absolute top-0 right-0 w-24 h-24 bg-white opacity-5 rounded-full -translate-y-1/2 translate-x-1/2" />
          <div className="flex items-center justify-between mb-4">
            <span className="text-rose-100 text-sm font-medium">Symptoms</span>
            <div className="w-9 h-9 bg-white bg-opacity-20 rounded-xl flex items-center justify-center">
              <AlertCircle size={18} className="text-white" />
            </div>
          </div>
          <p className="text-4xl font-bold">{symptoms?.length || 0}</p>
          <p className="text-rose-200 text-xs mt-2">Last 30 days</p>
        </div>

        {/* Streak */}
        <div className="stat-card-teal rounded-2xl p-6 text-white relative overflow-hidden">
          <div className="absolute top-0 right-0 w-24 h-24 bg-white opacity-5 rounded-full -translate-y-1/2 translate-x-1/2" />
          <div className="flex items-center justify-between mb-4">
            <span className="text-teal-100 text-sm font-medium">Log Streak</span>
            <div className="w-9 h-9 bg-white bg-opacity-20 rounded-xl flex items-center justify-center">
              <Flame size={18} className="text-white" />
            </div>
          </div>
          <p className="text-4xl font-bold">{streak}<span className="text-xl text-teal-200 font-normal">d</span></p>
          <p className="text-teal-200 text-xs mt-2 flex items-center gap-1">
            <Activity size={12} />
            {streak > 0 ? `${totalLogs} logs total` : 'Start logging!'}
          </p>
        </div>
      </div>

      {/* Charts */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6 mb-6">
        {/* Mood Trend */}
        <div className="chart-card p-6">
          <div className="flex items-center justify-between mb-6">
            <div>
              <h3 className="font-bold text-gray-800 text-lg">Mood Trend</h3>
              <p className="text-gray-400 text-xs mt-0.5">Daily mood scores — last 30 days</p>
            </div>
            <span className="health-badge bg-amber-100 text-amber-700">😊 {avgMoodStr}/10</span>
          </div>
          {moodChartData.length > 0 ? (
            <ResponsiveContainer width="100%" height={200}>
              <AreaChart data={moodChartData}>
                <defs>
                  <linearGradient id="moodGrad" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#f59e0b" stopOpacity={0.3} />
                    <stop offset="95%" stopColor="#f59e0b" stopOpacity={0} />
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="#f1f5f9" />
                <XAxis dataKey="date" tick={{ fontSize: 10, fill: '#94a3b8' }} tickLine={false} />
                <YAxis domain={[0, 10]} tick={{ fontSize: 10, fill: '#94a3b8' }} tickLine={false} axisLine={false} />
                <Tooltip contentStyle={{ borderRadius: 12, border: 'none', boxShadow: '0 4px 20px rgba(0,0,0,0.1)', fontSize: 13 }} />
                <Area type="monotone" dataKey="mood" stroke="#f59e0b" strokeWidth={2.5} fill="url(#moodGrad)" dot={false} />
              </AreaChart>
            </ResponsiveContainer>
          ) : (
            <div className="h-48 flex flex-col items-center justify-center text-gray-300 gap-2">
              <Smile size={36} />
              <p className="text-sm">Log your mood to see trends</p>
            </div>
          )}
        </div>

        {/* Sleep Chart */}
        <div className="chart-card p-6">
          <div className="flex items-center justify-between mb-6">
            <div>
              <h3 className="font-bold text-gray-800 text-lg">Sleep Hours</h3>
              <p className="text-gray-400 text-xs mt-0.5">Nightly sleep — last 30 days</p>
            </div>
            <span className="health-badge bg-purple-100 text-purple-700">🌙 {avgSleepStr}hrs avg</span>
          </div>
          {sleepChartData.length > 0 ? (
            <ResponsiveContainer width="100%" height={200}>
              <BarChart data={sleepChartData}>
                <defs>
                  <linearGradient id="sleepGrad" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="0%" stopColor="#8b5cf6" />
                    <stop offset="100%" stopColor="#6d28d9" />
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" stroke="#f1f5f9" />
                <XAxis dataKey="date" tick={{ fontSize: 10, fill: '#94a3b8' }} tickLine={false} />
                <YAxis domain={[0, 12]} tick={{ fontSize: 10, fill: '#94a3b8' }} tickLine={false} axisLine={false} />
                <Tooltip contentStyle={{ borderRadius: 12, border: 'none', boxShadow: '0 4px 20px rgba(0,0,0,0.1)', fontSize: 13 }} />
                <Bar dataKey="hours" fill="url(#sleepGrad)" radius={[6, 6, 0, 0]} />
              </BarChart>
            </ResponsiveContainer>
          ) : (
            <div className="h-48 flex flex-col items-center justify-center text-gray-300 gap-2">
              <Moon size={36} />
              <p className="text-sm">Log your sleep to see trends</p>
            </div>
          )}
        </div>
      </div>

      {/* Recent Symptoms */}
      <div className="chart-card p-6">
        <div className="flex items-center justify-between mb-6">
          <div>
            <h3 className="font-bold text-gray-800 text-lg">Recent Symptoms</h3>
            <p className="text-gray-400 text-xs mt-0.5">Latest tracked health events</p>
          </div>
          {symptoms && symptoms.length > 0 && (
            <span className="health-badge bg-rose-100 text-rose-700">{symptoms.length} tracked</span>
          )}
        </div>
        {symptoms && symptoms.length > 0 ? (
          <div className="space-y-3">
            {symptoms.slice(0, 5).map(symptom => (
              <div key={symptom.id} className="flex items-center justify-between p-4 rounded-xl hover:bg-gray-50 transition-colors">
                <div className="flex items-center gap-4">
                  <div className={`w-10 h-10 rounded-xl flex items-center justify-center ${
                    symptom.severity >= 7 ? 'bg-red-100' : symptom.severity >= 4 ? 'bg-amber-100' : 'bg-emerald-100'
                  }`}>
                    <AlertCircle size={18} className={
                      symptom.severity >= 7 ? 'text-red-500' : symptom.severity >= 4 ? 'text-amber-500' : 'text-emerald-500'
                    } />
                  </div>
                  <div>
                    <p className="font-semibold text-gray-800">{symptom.symptomName}</p>
                    <p className="text-xs text-gray-400">{symptom.logDate}</p>
                  </div>
                </div>
                <span className={`health-badge ${
                  symptom.severity >= 7 ? 'bg-red-100 text-red-700' :
                  symptom.severity >= 4 ? 'bg-amber-100 text-amber-700' :
                  'bg-emerald-100 text-emerald-700'
                }`}>
                  {symptom.severity >= 7 ? '🔴' : symptom.severity >= 4 ? '🟡' : '🟢'} {symptom.severity}/10
                </span>
              </div>
            ))}
          </div>
        ) : (
          <div className="py-12 text-center">
            <AlertCircle size={40} className="mx-auto text-gray-200 mb-3" />
            <p className="text-gray-400">No symptoms logged yet</p>
            <p className="text-gray-300 text-sm mt-1">Use Daily Logs to start tracking</p>
          </div>
        )}
      </div>
    </div>
  )
}