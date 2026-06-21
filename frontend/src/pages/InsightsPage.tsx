import React, { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { getInsights } from '../api/logs'
import { Brain, Loader, Calendar, Sparkles, TrendingUp, Clock } from 'lucide-react'

export default function InsightsPage() {
  const [from, setFrom] = useState<string>(() => {
    const d = new Date(); d.setDate(d.getDate() - 30); return d.toISOString().split('T')[0];
  });
  const [to, setTo] = useState<string>(() => new Date().toISOString().split('T')[0]);
  const [enabled, setEnabled] = useState(false);

  const { data, isLoading, error, refetch } = useQuery({
    queryKey: ['insights', from, to],
    queryFn: () => getInsights(from, to),
    enabled,
    refetchOnWindowFocus: false,
  });

  const handleAnalyze = () => {
    if (enabled) refetch(); else setEnabled(true);
  };

  const renderInsights = (text: string) => {
    const lines = text.split('\n');
    const elements: React.ReactNode[] = [];
    let key = 0;

    const renderInline = (str: string) => {
      const parts = str.split(/(\*\*[^*]+\*\*)/g);
      return parts.map((part, i) => {
        if (part.startsWith('**') && part.endsWith('**')) {
          return <strong key={i} className="font-semibold text-gray-900">{part.slice(2, -2)}</strong>;
        }
        return part;
      });
    };

    lines.forEach((line) => {
      const trimmed = line.trim();

      if (trimmed === '') {
        elements.push(<div key={key++} className="h-1" />);
        return;
      }

      // Numbered item: "1. **Title**:" or "1. Some text"
      const numberedMatch = trimmed.match(/^(\d+)\.\s+(.*)$/);
      if (numberedMatch) {
        const num = numberedMatch[1];
        const content = numberedMatch[2];
        elements.push(
          <div key={key++} className="flex items-start gap-3 p-3 rounded-xl bg-white/70 shadow-sm border border-indigo-100/60">
            <span className="flex-shrink-0 w-7 h-7 rounded-full flex items-center justify-center text-xs font-bold text-white"
              style={{ background: 'linear-gradient(135deg, #6366f1, #8b5cf6)' }}>
              {num}
            </span>
            <p className="text-sm font-medium text-gray-800 leading-relaxed pt-0.5">{renderInline(content)}</p>
          </div>
        );
        return;
      }

      // Sub-bullet: "- something" or "• something"
      const bulletMatch = trimmed.match(/^[-•]\s+(.*)$/);
      if (bulletMatch) {
        elements.push(
          <div key={key++} className="flex items-start gap-2.5 pl-4">
            <span className="flex-shrink-0 w-1.5 h-1.5 rounded-full bg-indigo-400 mt-1.5" />
            <p className="text-sm text-gray-600 leading-relaxed">{renderInline(bulletMatch[1])}</p>
          </div>
        );
        return;
      }

      // Plain paragraph
      elements.push(
        <p key={key++} className="text-sm text-gray-700 leading-relaxed">{renderInline(trimmed)}</p>
      );
    });

    return elements;
  };

  return (
    <div className="p-8">
      {/* Header */}
      <div className="mb-8">
        <div className="flex items-center gap-2 mb-1">
          <Sparkles size={16} className="text-indigo-500" />
          <span className="text-sm font-semibold text-indigo-600 uppercase tracking-wider">AI-Powered</span>
        </div>
        <h1 className="text-3xl font-bold text-gray-900">Health Insights</h1>
        <p className="text-gray-500 mt-1">Let AI analyze your health patterns and trends</p>
      </div>

      {/* Date range card */}
      <div className="chart-card p-6 mb-6">
        <h3 className="font-semibold text-gray-700 mb-4 flex items-center gap-2">
          <Calendar size={16} className="text-indigo-500" />
          Select Analysis Period
        </h3>
        <div className="flex items-center gap-4 flex-wrap">
          <div className="flex items-center gap-2">
            <label className="text-sm font-medium text-gray-600">From</label>
            <input
              type="date" value={from} onChange={(e) => setFrom(e.target.value)}
              className="px-3 py-2 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 text-sm bg-gray-50"
            />
          </div>
          <div className="flex items-center gap-2">
            <label className="text-sm font-medium text-gray-600">To</label>
            <input
              type="date" value={to} onChange={(e) => setTo(e.target.value)}
              className="px-3 py-2 border border-gray-200 rounded-xl focus:outline-none focus:ring-2 focus:ring-indigo-500 text-sm bg-gray-50"
            />
          </div>
          <button
            onClick={handleAnalyze}
            className="flex items-center gap-2 px-6 py-2.5 rounded-xl font-semibold text-white transition-all"
            style={{ background: 'linear-gradient(135deg, #6366f1, #8b5cf6)', boxShadow: '0 4px 15px rgba(99,102,241,0.4)' }}
          >
            <Brain size={17} />
            {isLoading ? 'Analyzing...' : 'Analyze with AI'}
          </button>
        </div>
      </div>

      {/* Loading */}
      {isLoading && (
        <div className="chart-card p-16 text-center">
          <div className="w-20 h-20 mx-auto mb-6 rounded-full flex items-center justify-center"
            style={{ background: 'linear-gradient(135deg, #6366f1, #8b5cf6)', boxShadow: '0 10px 30px rgba(99,102,241,0.3)' }}>
            <Brain size={36} className="text-white" />
          </div>
          <Loader className="animate-spin mx-auto mb-4 text-indigo-500" size={28} />
          <p className="text-gray-700 font-semibold text-lg">AI is analyzing your health data...</p>
          <p className="text-gray-400 text-sm mt-2">Scanning mood patterns, sleep quality, symptoms & meals</p>
          <div className="flex items-center justify-center gap-2 mt-4">
            <Clock size={14} className="text-gray-400" />
            <span className="text-xs text-gray-400">Usually takes 5–10 seconds</span>
          </div>
        </div>
      )}

      {/* Error */}
      {error && (
        <div className="p-6 rounded-2xl bg-red-50 border border-red-100 text-red-600">
          <p className="font-semibold">Analysis failed</p>
          <p className="text-sm mt-1">Make sure you have health data logged first, then try again.</p>
        </div>
      )}

      {/* Results */}
      {data && !isLoading && (
        <div className="space-y-5 fade-in-up">
          {/* Stats bar */}
          <div className="grid grid-cols-3 gap-4">
            <div className="chart-card p-5 text-center">
              <p className="text-3xl font-bold text-indigo-600">{data.totalLogsAnalyzed}</p>
              <p className="text-xs text-gray-500 font-semibold uppercase tracking-wider mt-1">Logs Analyzed</p>
            </div>
            <div className="chart-card p-5 text-center">
              <p className="text-lg font-bold text-gray-800">{data.from} → {data.to}</p>
              <p className="text-xs text-gray-500 font-semibold uppercase tracking-wider mt-1">Period</p>
            </div>
            <div className="chart-card p-5 text-center">
              <p className="text-sm font-bold text-gray-800">{new Date(data.generatedAt).toLocaleTimeString()}</p>
              <p className="text-xs text-gray-500 font-semibold uppercase tracking-wider mt-1">Generated At</p>
            </div>
          </div>

          {/* AI Analysis */}
          <div className="chart-card p-8">
            <div className="flex items-center gap-3 mb-6">
              <div className="w-10 h-10 rounded-xl flex items-center justify-center"
                style={{ background: 'linear-gradient(135deg, #6366f1, #8b5cf6)' }}>
                <Brain size={20} className="text-white" />
              </div>
              <div>
                <h3 className="font-bold text-gray-900 text-lg">AI Health Analysis</h3>
                <p className="text-xs text-gray-400">Powered by Llama 3.1</p>
              </div>
              <span className="ml-auto health-badge bg-indigo-100 text-indigo-700">
                <TrendingUp size={12} /> Personalized
              </span>
            </div>
            <div className="p-6 rounded-xl bg-gradient-to-br from-indigo-50 to-purple-50 border border-indigo-100 space-y-3">
              {renderInsights(data.insights)}
            </div>
          </div>
        </div>
      )}

      {/* Empty state */}
      {!enabled && !isLoading && (
        <div className="chart-card p-16 text-center">
          <div className="w-24 h-24 mx-auto mb-6 rounded-3xl flex items-center justify-center"
            style={{ background: 'linear-gradient(135deg, #e0e7ff, #ede9fe)' }}>
            <Brain size={44} className="text-indigo-400" />
          </div>
          <h3 className="text-xl font-bold text-gray-800 mb-2">Ready to Analyze</h3>
          <p className="text-gray-400 max-w-sm mx-auto">
            Select a date range above and click <strong>"Analyze with AI"</strong> to get personalized health insights powered by AI
          </p>
        </div>
      )}
    </div>
  )
}