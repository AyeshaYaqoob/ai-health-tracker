import { useState } from 'react'
import { useAuth } from '../hooks/useAuth'
import { useQuery } from '@tanstack/react-query'
import { getSymptomLogs, getMoodLogs, getSleepLogs, getMealLogs } from '../api/logs'
import { useNavigate } from 'react-router-dom'
import {
  User, Mail, Shield, Activity, Smile, Moon, Utensils,
  LogOut, Star, CalendarDays, Zap, Lock
} from 'lucide-react'
import toast from 'react-hot-toast'

export default function ProfilePage() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()

  const [today] = useState(() => new Date().toISOString().split('T')[0])
  const [thirtyDaysAgo] = useState(() => new Date(Date.now() - 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0])

  const { data: symptoms } = useQuery({ queryKey: ['symptoms', thirtyDaysAgo, today], queryFn: () => getSymptomLogs(thirtyDaysAgo, today) })
  const { data: moods } = useQuery({ queryKey: ['moods', thirtyDaysAgo, today], queryFn: () => getMoodLogs(thirtyDaysAgo, today) })
  const { data: sleep } = useQuery({ queryKey: ['sleep', thirtyDaysAgo, today], queryFn: () => getSleepLogs(thirtyDaysAgo, today) })
  const { data: meals } = useQuery({ queryKey: ['meals', thirtyDaysAgo, today], queryFn: () => getMealLogs(thirtyDaysAgo, today) })

  const totalLogs = (symptoms?.length ?? 0) + (moods?.length ?? 0) + (sleep?.length ?? 0) + (meals?.length ?? 0)
  const avgMood = moods?.length ? (moods.reduce((s, m) => s + m.moodScore, 0) / moods.length).toFixed(1) : 'N/A'
  const avgSleep = sleep?.length ? (sleep.reduce((s, sl) => s + sl.hoursSlept, 0) / sleep.length).toFixed(1) : 'N/A'

  const handleLogout = () => {
    logout()
    toast.success('Signed out. See you soon! 👋')
    navigate('/login')
  }

  const initials = `${user?.firstName?.[0] ?? ''}${user?.lastName?.[0] ?? ''}`

  const stats = [
    { icon: Activity, label: 'Symptoms Logged', value: symptoms?.length ?? 0, color: 'text-rose-500', bg: 'bg-rose-50' },
    { icon: Smile, label: 'Mood Entries', value: moods?.length ?? 0, color: 'text-amber-500', bg: 'bg-amber-50' },
    { icon: Moon, label: 'Sleep Records', value: sleep?.length ?? 0, color: 'text-violet-500', bg: 'bg-violet-50' },
    { icon: Utensils, label: 'Meals Logged', value: meals?.length ?? 0, color: 'text-emerald-500', bg: 'bg-emerald-50' },
  ]

  return (
    <div className="p-8 max-w-3xl mx-auto fade-in-up">
      {/* Header */}
      <div className="mb-8">
        <div className="flex items-center gap-2 mb-1">
          <Zap size={16} className="text-indigo-500" />
          <span className="text-sm font-semibold text-indigo-600 uppercase tracking-wider">Your Account</span>
        </div>
        <h1 className="text-3xl font-bold text-gray-900">Profile</h1>
        <p className="text-gray-500 mt-1">Manage your health tracker account</p>
      </div>

      {/* Profile card */}
      <div className="chart-card p-8 mb-6">
        <div className="flex items-center gap-6 mb-8">
          {/* Avatar */}
          <div className="w-20 h-20 rounded-2xl flex items-center justify-center text-white text-2xl font-bold shadow-lg"
            style={{ background: 'linear-gradient(135deg, #6366f1, #8b5cf6)' }}>
            {initials}
          </div>
          <div>
            <h2 className="text-2xl font-bold text-gray-900">
              {user?.firstName} {user?.lastName}
            </h2>
            <p className="text-gray-500 flex items-center gap-1.5 mt-1">
              <Mail size={14} /> {user?.email}
            </p>
            <div className="flex items-center gap-2 mt-2">
              <span className="health-badge bg-indigo-100 text-indigo-700">
                <Star size={11} /> Premium Health Tracker
              </span>
              <span className="health-badge bg-emerald-100 text-emerald-700">
                <Shield size={11} /> Verified
              </span>
            </div>
          </div>
        </div>

        {/* Info rows */}
        <div className="space-y-3">
          {[
            { icon: User, label: 'Full Name', value: `${user?.firstName} ${user?.lastName}` },
            { icon: Mail, label: 'Email Address', value: user?.email ?? '—' },
            { icon: CalendarDays, label: 'Tracking Period', value: 'Last 30 days' },
          ].map(({ icon: Icon, label, value }) => (
            <div key={label} className="flex items-center justify-between p-4 rounded-xl bg-gray-50 hover:bg-gray-100 transition-colors">
              <div className="flex items-center gap-3">
                <div className="w-8 h-8 rounded-lg bg-indigo-100 flex items-center justify-center">
                  <Icon size={15} className="text-indigo-600" />
                </div>
                <span className="text-sm font-medium text-gray-600">{label}</span>
              </div>
              <span className="text-sm font-semibold text-gray-800">{value}</span>
            </div>
          ))}
        </div>
      </div>

      {/* Stats */}
      <div className="chart-card p-6 mb-6">
        <h3 className="font-bold text-gray-800 text-lg mb-5 flex items-center gap-2">
          <Activity size={18} className="text-indigo-500" />
          30-Day Activity Summary
        </h3>
        <div className="grid grid-cols-2 gap-4 mb-4">
          {stats.map(({ icon: Icon, label, value, color, bg }) => (
            <div key={label} className={`${bg} rounded-xl p-4 flex items-center gap-4`}>
              <div className="w-10 h-10 bg-white rounded-lg flex items-center justify-center shadow-sm">
                <Icon size={18} className={color} />
              </div>
              <div>
                <p className="text-2xl font-bold text-gray-900">{value}</p>
                <p className="text-xs text-gray-500 font-medium">{label}</p>
              </div>
            </div>
          ))}
        </div>
        {/* Summary row */}
        <div className="grid grid-cols-3 gap-3 pt-4 border-t border-gray-100">
          <div className="text-center">
            <p className="text-2xl font-bold text-indigo-600">{totalLogs}</p>
            <p className="text-xs text-gray-400 font-medium mt-0.5">Total Logs</p>
          </div>
          <div className="text-center border-x border-gray-100">
            <p className="text-2xl font-bold text-amber-500">{avgMood}<span className="text-sm font-normal text-gray-400">/10</span></p>
            <p className="text-xs text-gray-400 font-medium mt-0.5">Avg Mood</p>
          </div>
          <div className="text-center">
            <p className="text-2xl font-bold text-violet-500">{avgSleep}<span className="text-sm font-normal text-gray-400">hrs</span></p>
            <p className="text-xs text-gray-400 font-medium mt-0.5">Avg Sleep</p>
          </div>
        </div>
      </div>

      {/* Actions */}
      <div className="chart-card p-6">
        <h3 className="font-bold text-gray-800 text-lg mb-4">Account Actions</h3>
        <div className="space-y-3">
          {/* Password change placeholder */}
          <div className="flex items-center justify-between p-4 rounded-xl border border-gray-200 bg-gray-50">
            <div className="flex items-center gap-3">
              <div className="w-8 h-8 rounded-lg bg-blue-100 flex items-center justify-center">
                <Lock size={15} className="text-blue-600" />
              </div>
              <div>
                <p className="text-sm font-semibold text-gray-700">Change Password</p>
                <p className="text-xs text-gray-400">Coming soon</p>
              </div>
            </div>
            <span className="px-3 py-1 rounded-lg text-xs font-semibold bg-gray-200 text-gray-500">Soon</span>
          </div>

          {/* Sign out */}
          <button
            onClick={handleLogout}
            className="w-full flex items-center gap-3 p-4 rounded-xl border border-red-200 bg-red-50 hover:bg-red-100 transition-all group"
          >
            <div className="w-8 h-8 rounded-lg bg-red-100 flex items-center justify-center">
              <LogOut size={15} className="text-red-500 group-hover:text-red-600" />
            </div>
            <div className="text-left">
              <p className="text-sm font-semibold text-red-600">Sign Out</p>
              <p className="text-xs text-red-400">End your current session</p>
            </div>
          </button>
        </div>
      </div>
    </div>
  )
}
