import { useQuery } from '@tanstack/react-query'
import { getAlerts } from '../api/logs'
import type { HealthAlert } from '../types/index'
import {
  Bell, ShieldAlert, CheckCircle2, Info, AlertTriangle,
  Loader, RefreshCw, Activity, Moon, Smile, Syringe, Sparkles
} from 'lucide-react'

const alertConfig = {
  danger: {
    bg: 'bg-red-50',
    border: 'border-red-200',
    icon: ShieldAlert,
    iconColor: 'text-red-500',
    iconBg: 'bg-red-100',
    badge: 'bg-red-100 text-red-700',
    bar: 'bg-red-400',
  },
  warning: {
    bg: 'bg-amber-50',
    border: 'border-amber-200',
    icon: AlertTriangle,
    iconColor: 'text-amber-500',
    iconBg: 'bg-amber-100',
    badge: 'bg-amber-100 text-amber-700',
    bar: 'bg-amber-400',
  },
  success: {
    bg: 'bg-emerald-50',
    border: 'border-emerald-200',
    icon: CheckCircle2,
    iconColor: 'text-emerald-500',
    iconBg: 'bg-emerald-100',
    badge: 'bg-emerald-100 text-emerald-700',
    bar: 'bg-emerald-400',
  },
  info: {
    bg: 'bg-blue-50',
    border: 'border-blue-200',
    icon: Info,
    iconColor: 'text-blue-500',
    iconBg: 'bg-blue-100',
    badge: 'bg-blue-100 text-blue-700',
    bar: 'bg-blue-400',
  },
}

const categoryIcon = (cat: string) => {
  switch (cat) {
    case 'mood': return <Smile size={13} />
    case 'sleep': return <Moon size={13} />
    case 'symptom': return <Syringe size={13} />
    default: return <Activity size={13} />
  }
}

function AlertCard({ alert }: { alert: HealthAlert }) {
  const cfg = alertConfig[alert.severity] ?? alertConfig.info
  const Icon = cfg.icon

  return (
    <div className={`relative overflow-hidden rounded-2xl border ${cfg.border} ${cfg.bg} p-5 flex gap-4 transition-all hover:shadow-md`}>
      {/* Left accent bar */}
      <div className={`absolute left-0 top-0 bottom-0 w-1 ${cfg.bar} rounded-l-2xl`} />

      {/* Icon */}
      <div className={`flex-shrink-0 w-11 h-11 rounded-xl ${cfg.iconBg} flex items-center justify-center`}>
        <Icon size={22} className={cfg.iconColor} />
      </div>

      {/* Content */}
      <div className="flex-1 min-w-0">
        <div className="flex items-center gap-2 mb-1.5 flex-wrap">
          <h3 className="font-bold text-gray-900 text-sm">{alert.title}</h3>
          <span className={`inline-flex items-center gap-1 px-2 py-0.5 rounded-full text-xs font-semibold ${cfg.badge}`}>
            {categoryIcon(alert.category)}
            {alert.category}
          </span>
          <span className={`ml-auto inline-flex px-2 py-0.5 rounded-full text-xs font-bold uppercase tracking-wide ${cfg.badge}`}>
            {alert.severity}
          </span>
        </div>
        <p className="text-sm text-gray-600 leading-relaxed">{alert.message}</p>
      </div>
    </div>
  )
}

export default function AlertsPage() {
  const { data, isLoading, error, refetch, isFetching } = useQuery({
    queryKey: ['alerts'],
    queryFn: getAlerts,
    staleTime: 1000 * 60 * 5, // 5 min cache
  })

  const dangerCount = data?.alerts.filter(a => a.severity === 'danger').length ?? 0
  const warningCount = data?.alerts.filter(a => a.severity === 'warning').length ?? 0
  const successCount = data?.alerts.filter(a => a.severity === 'success').length ?? 0
  const infoCount = data?.alerts.filter(a => a.severity === 'info').length ?? 0

  return (
    <div className="p-8 fade-in-up">
      {/* Header */}
      <div className="mb-8 flex items-start justify-between">
        <div>
          <div className="flex items-center gap-2 mb-1">
            <Sparkles size={16} className="text-rose-500" />
            <span className="text-sm font-semibold text-rose-600 uppercase tracking-wider">AI-Powered</span>
          </div>
          <h1 className="text-3xl font-bold text-gray-900">Health Alerts</h1>
          <p className="text-gray-500 mt-1">Smart pattern detection from your last 7 days of data</p>
        </div>
        <button
          onClick={() => refetch()}
          disabled={isFetching}
          className="flex items-center gap-2 px-4 py-2 rounded-xl border border-gray-200 bg-white text-gray-600 hover:bg-gray-50 transition-all text-sm font-medium"
        >
          <RefreshCw size={15} className={isFetching ? 'animate-spin' : ''} />
          Refresh
        </button>
      </div>

      {/* Loading */}
      {isLoading && (
        <div className="chart-card p-16 text-center">
          <div className="w-16 h-16 mx-auto mb-5 rounded-2xl flex items-center justify-center"
            style={{ background: 'linear-gradient(135deg, #f43f5e, #e11d48)', boxShadow: '0 10px 30px rgba(244,63,94,0.3)' }}>
            <Bell size={30} className="text-white" />
          </div>
          <Loader className="animate-spin mx-auto mb-3 text-rose-500" size={24} />
          <p className="text-gray-700 font-semibold">Analyzing your health patterns...</p>
          <p className="text-gray-400 text-sm mt-1">Checking mood, sleep & symptom trends</p>
        </div>
      )}

      {/* Error */}
      {error && (
        <div className="p-6 rounded-2xl bg-red-50 border border-red-100 text-red-600">
          <p className="font-semibold">⚠️ Could not load alerts</p>
          <p className="text-sm mt-1">Make sure the backend is running and you have logged health data.</p>
        </div>
      )}

      {/* Stats summary */}
      {data && !isLoading && (
        <div className="space-y-5">
          {/* Summary badges */}
          <div className="grid grid-cols-2 sm:grid-cols-4 gap-4 mb-6">
            {[
              { label: 'Critical', count: dangerCount, color: 'from-red-400 to-rose-500', shadow: 'rgba(244,63,94,0.35)' },
              { label: 'Warnings', count: warningCount, color: 'from-amber-400 to-orange-500', shadow: 'rgba(245,158,11,0.35)' },
              { label: 'Positive', count: successCount, color: 'from-emerald-400 to-green-500', shadow: 'rgba(16,185,129,0.35)' },
              { label: 'Info', count: infoCount, color: 'from-blue-400 to-sky-500', shadow: 'rgba(59,130,246,0.35)' },
            ].map(({ label, count, color, shadow }) => (
              <div
                key={label}
                className={`rounded-2xl p-5 text-white bg-gradient-to-br ${color} relative overflow-hidden`}
                style={{ boxShadow: `0 10px 30px ${shadow}` }}
              >
                <div className="absolute top-0 right-0 w-16 h-16 bg-white opacity-10 rounded-full -translate-y-1/2 translate-x-1/2" />
                <p className="text-3xl font-bold">{count}</p>
                <p className="text-sm font-medium opacity-80 mt-0.5">{label}</p>
              </div>
            ))}
          </div>

          {/* Period */}
          <div className="flex items-center gap-2 text-xs text-gray-400 mb-2">
            <Activity size={12} />
            Analysis period: <strong className="text-gray-600">{data.period.from}</strong> → <strong className="text-gray-600">{data.period.to}</strong>
            <span className="ml-auto">Generated {new Date(data.generatedAt).toLocaleTimeString()}</span>
          </div>

          {/* Alert cards */}
          {data.alerts.length === 0 ? (
            <div className="chart-card p-16 text-center">
              <div className="w-20 h-20 mx-auto mb-5 rounded-3xl flex items-center justify-center"
                style={{ background: 'linear-gradient(135deg, #d1fae5, #a7f3d0)' }}>
                <CheckCircle2 size={40} className="text-emerald-500" />
              </div>
              <h3 className="text-xl font-bold text-gray-800 mb-2">All Clear!</h3>
              <p className="text-gray-400 max-w-sm mx-auto">No health alerts for the past 7 days. Keep logging data to enable pattern detection.</p>
            </div>
          ) : (
            <div className="space-y-3">
              {/* Danger alerts first */}
              {['danger', 'warning', 'success', 'info'].map(severity =>
                data.alerts
                  .filter(a => a.severity === severity)
                  .map((alert, i) => <AlertCard key={`${severity}-${i}`} alert={alert} />)
              )}
            </div>
          )}
        </div>
      )}

      {/* Empty state (not loading, no data) */}
      {!isLoading && !data && !error && (
        <div className="chart-card p-16 text-center">
          <div className="w-20 h-20 mx-auto mb-5 rounded-3xl flex items-center justify-center"
            style={{ background: 'linear-gradient(135deg, #fee2e2, #fecaca)' }}>
            <Bell size={36} className="text-rose-400" />
          </div>
          <h3 className="text-xl font-bold text-gray-800 mb-2">Loading Alerts...</h3>
          <p className="text-gray-400">Your AI health alerts will appear here.</p>
        </div>
      )}
    </div>
  )
}
