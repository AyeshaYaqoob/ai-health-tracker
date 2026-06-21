import { useQuery, useMutation } from '@tanstack/react-query'
import type { JSX } from 'react'

// Parse AI text: numbered items + **bold** into styled JSX
function formatInsights(text: string): JSX.Element {
  if (!text) return <p className="text-gray-400 text-sm">No insights available.</p>

  // Split by numbered list items like "1. " or "\n1."
  const lines = text.split(/\n/)
  const bullets: string[] = []
  let current = ''

  for (const line of lines) {
    const isNumbered = /^\d+\.\s/.test(line.trim())
    if (isNumbered) {
      if (current.trim()) bullets.push(current.trim())
      current = line.trim().replace(/^\d+\.\s*/, '')
    } else if (line.trim()) {
      current += ' ' + line.trim()
    }
  }
  if (current.trim()) bullets.push(current.trim())

  // If no numbered items found, split by sentence for bullet fallback
  const items = bullets.length > 0 ? bullets : text.split(/(?<=\.\s)/).filter(s => s.trim().length > 10)

  // Render **bold** inside text
  const renderBold = (str: string) => {
    const parts = str.split(/\*\*(.*?)\*\*/g)
    return parts.map((part, i) =>
      i % 2 === 1
        ? <strong key={i} className="text-gray-900 font-semibold">{part}</strong>
        : <span key={i}>{part}</span>
    )
  }

  const colors = [
    { bg: 'bg-indigo-100', text: 'text-indigo-700', border: 'border-indigo-200' },
    { bg: 'bg-emerald-100', text: 'text-emerald-700', border: 'border-emerald-200' },
    { bg: 'bg-amber-100', text: 'text-amber-700', border: 'border-amber-200' },
    { bg: 'bg-rose-100', text: 'text-rose-700', border: 'border-rose-200' },
    { bg: 'bg-purple-100', text: 'text-purple-700', border: 'border-purple-200' },
    { bg: 'bg-sky-100', text: 'text-sky-700', border: 'border-sky-200' },
  ]

  return (
    <div className="space-y-3">
      {items.map((item, i) => {
        const color = colors[i % colors.length]
        return (
          <div key={i} className={`flex gap-3 p-4 rounded-xl border ${color.border} bg-white`}>
            <div className={`w-7 h-7 rounded-lg ${color.bg} flex items-center justify-center flex-shrink-0 mt-0.5`}>
              <span className={`text-xs font-bold ${color.text}`}>{i + 1}</span>
            </div>
            <p className="text-gray-700 text-sm leading-relaxed">{renderBold(item)}</p>
          </div>
        )
      })}
    </div>
  )
}
import { getWeeklyReports } from '../api/logs'
import { FileText, Calendar, Smile, Moon, Loader, Sparkles, TrendingUp, AlertCircle } from 'lucide-react'
import toast from 'react-hot-toast'
import client from '../api/client'

export default function WeeklyReportsPage() {
  const { data: reports, isLoading, refetch } = useQuery({
    queryKey: ['weekly-reports'],
    queryFn: getWeeklyReports
  })

  const generateMutation = useMutation({
    mutationFn: () => client.post('/api/weekly-reports/generate'),
    onSuccess: () => { toast.success('Weekly report generated! 🎉'); refetch() },
    onError: () => toast.error('Failed to generate report')
  })

  return (
    <div className="p-8">
      {/* Header */}
      <div className="flex items-center justify-between mb-8">
        <div>
          <div className="flex items-center gap-2 mb-1">
            <Sparkles size={16} className="text-emerald-500" />
            <span className="text-sm font-semibold text-emerald-600 uppercase tracking-wider">AI Generated</span>
          </div>
          <h1 className="text-3xl font-bold text-gray-900">Weekly Reports</h1>
          <p className="text-gray-500 mt-1">AI-powered summaries of your health journey</p>
        </div>
        <button
          onClick={() => generateMutation.mutate()}
          disabled={generateMutation.isPending}
          className="flex items-center gap-2 px-6 py-3 rounded-xl font-semibold text-white transition-all disabled:opacity-50"
          style={{ background: 'linear-gradient(135deg, #10b981, #059669)', boxShadow: '0 4px 15px rgba(16,185,129,0.4)' }}
        >
          {generateMutation.isPending
            ? <><Loader size={17} className="animate-spin" /> Generating...</>
            : <><FileText size={17} /> Generate Report</>
          }
        </button>
      </div>

      {/* Loading */}
      {isLoading && (
        <div className="text-center py-16">
          <Loader className="animate-spin mx-auto text-indigo-500 mb-4" size={32} />
          <p className="text-gray-400">Loading your reports...</p>
        </div>
      )}

      {/* Empty state */}
      {reports && reports.length === 0 && (
        <div className="chart-card p-16 text-center">
          <div className="w-24 h-24 mx-auto mb-6 rounded-3xl flex items-center justify-center"
            style={{ background: 'linear-gradient(135deg, #d1fae5, #a7f3d0)' }}>
            <FileText size={44} className="text-emerald-500" />
          </div>
          <h3 className="text-xl font-bold text-gray-800 mb-2">No Reports Yet</h3>
          <p className="text-gray-400 max-w-sm mx-auto">
            Click <strong>"Generate Report"</strong> to create your first AI-powered weekly health summary
          </p>
        </div>
      )}

      {/* Reports list */}
      <div className="space-y-6">
        {reports?.map(report => (
          <div key={report.id} className="chart-card p-6 fade-in-up">
            {/* Report header */}
            <div className="flex items-center justify-between mb-5">
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 rounded-xl flex items-center justify-center"
                  style={{ background: 'linear-gradient(135deg, #10b981, #059669)' }}>
                  <Calendar size={18} className="text-white" />
                </div>
                <div>
                  <h3 className="font-bold text-gray-900">
                    {new Date(report.weekStartDate).toLocaleDateString('en-US', { month: 'short', day: 'numeric' })} –{' '}
                    {new Date(report.weekEndDate).toLocaleDateString('en-US', { month: 'short', day: 'numeric', year: 'numeric' })}
                  </h3>
                  <p className="text-xs text-gray-400">
                    Generated {new Date(report.generatedAt).toLocaleDateString('en-US', { month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' })}
                  </p>
                </div>
              </div>
              <span className="health-badge bg-emerald-100 text-emerald-700">
                <TrendingUp size={12} /> Weekly Summary
              </span>
            </div>

            {/* Stats row */}
            <div className="grid grid-cols-3 gap-4 mb-5">
              <div className="rounded-xl p-4 text-center" style={{ background: 'linear-gradient(135deg, #fef3c7, #fde68a)' }}>
                <Smile size={20} className="mx-auto text-amber-600 mb-2" />
                <p className="text-xs text-amber-700 font-semibold uppercase tracking-wider">Avg Mood</p>
                <p className="text-2xl font-bold text-amber-800">{report.avgMoodScore.toFixed(1)}<span className="text-sm font-normal">/10</span></p>
              </div>
              <div className="rounded-xl p-4 text-center" style={{ background: 'linear-gradient(135deg, #ede9fe, #ddd6fe)' }}>
                <Moon size={20} className="mx-auto text-purple-600 mb-2" />
                <p className="text-xs text-purple-700 font-semibold uppercase tracking-wider">Avg Sleep</p>
                <p className="text-2xl font-bold text-purple-800">{report.avgSleepHours.toFixed(1)}<span className="text-sm font-normal">hrs</span></p>
              </div>
              <div className="rounded-xl p-4 text-center" style={{ background: 'linear-gradient(135deg, #fee2e2, #fecaca)' }}>
                <AlertCircle size={20} className="mx-auto text-rose-600 mb-2" />
                <p className="text-xs text-rose-700 font-semibold uppercase tracking-wider">Top Symptom</p>
                <p className="text-sm font-bold text-rose-800 mt-1">{report.topSymptoms || 'None'}</p>
              </div>
            </div>

            {/* AI Insights */}
            <div className="rounded-xl p-5" style={{ background: 'linear-gradient(135deg, #f0f9ff, #e0f2fe)', border: '1px solid #bae6fd' }}>
              <div className="flex items-center gap-2 mb-4">
                <Sparkles size={14} className="text-sky-600" />
                <p className="text-xs font-bold text-sky-600 uppercase tracking-wider">AI Insights</p>
              </div>
              {formatInsights(report.aiInsights)}
            </div>
          </div>
        ))}
      </div>
    </div>
  )
}