import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'
import { login as loginApi } from '../api/auth'
import toast from 'react-hot-toast'
import { Heart, Mail, Lock, Activity, Moon, Brain, Smile } from 'lucide-react'
import axios from 'axios'

export default function LoginPage() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const { login } = useAuth()
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    try {
      const data = await loginApi({ email, password })
      login(data)
      toast.success(`Welcome back, ${data.firstName}! 💪`)
      navigate('/')
    } catch (err) {
      if (axios.isAxiosError(err) && !err.response) {
        toast.error('Cannot connect to server.')
      } else {
        toast.error('Invalid email or password')
      }
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-screen flex">
      {/* Left Panel - Branding */}
      <div className="hidden lg:flex lg:w-1/2 auth-bg flex-col items-center justify-center p-12 relative">
        {/* Floating health metrics */}
        <div className="absolute top-20 left-10 glass-card p-4 rounded-2xl floating-icon">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 bg-emerald-500 bg-opacity-20 rounded-full flex items-center justify-center">
              <Activity size={20} className="text-emerald-400" />
            </div>
            <div>
              <p className="text-white text-xs opacity-60">Daily Steps</p>
              <p className="text-white font-bold">8,432</p>
            </div>
          </div>
        </div>

        <div className="absolute top-40 right-10 glass-card p-4 rounded-2xl floating-icon">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 bg-purple-500 bg-opacity-20 rounded-full flex items-center justify-center">
              <Moon size={20} className="text-purple-400" />
            </div>
            <div>
              <p className="text-white text-xs opacity-60">Sleep Quality</p>
              <p className="text-white font-bold">8.5 hrs ✨</p>
            </div>
          </div>
        </div>

        <div className="absolute bottom-40 left-10 glass-card p-4 rounded-2xl floating-icon">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 bg-rose-500 bg-opacity-20 rounded-full flex items-center justify-center">
              <Smile size={20} className="text-rose-400" />
            </div>
            <div>
              <p className="text-white text-xs opacity-60">Mood Score</p>
              <p className="text-white font-bold">9/10 😊</p>
            </div>
          </div>
        </div>

        <div className="absolute bottom-20 right-12 glass-card p-4 rounded-2xl floating-icon">
          <div className="flex items-center gap-3">
            <div className="w-10 h-10 bg-sky-500 bg-opacity-20 rounded-full flex items-center justify-center">
              <Brain size={20} className="text-sky-400" />
            </div>
            <div>
              <p className="text-white text-xs opacity-60">AI Insight</p>
              <p className="text-white font-bold">Trending Up 📈</p>
            </div>
          </div>
        </div>

        {/* Central branding */}
        <div className="text-center z-10">
          <div className="w-24 h-24 bg-white bg-opacity-10 rounded-3xl flex items-center justify-center mx-auto mb-6 backdrop-blur-sm border border-white border-opacity-20">
            <Heart size={48} className="text-rose-400" fill="rgba(251,113,133,0.4)" />
          </div>
          <h1 className="text-4xl font-bold text-white mb-3">HealthTracker AI</h1>
          <p className="text-blue-200 text-lg max-w-xs leading-relaxed">
            Your personal AI-powered health companion for a better life
          </p>
          <div className="flex items-center justify-center gap-6 mt-8">
            <div className="text-center">
              <p className="text-white text-2xl font-bold">30+</p>
              <p className="text-blue-300 text-sm">Days Tracked</p>
            </div>
            <div className="w-px h-10 bg-white opacity-20" />
            <div className="text-center">
              <p className="text-white text-2xl font-bold">AI</p>
              <p className="text-blue-300 text-sm">Insights</p>
            </div>
            <div className="w-px h-10 bg-white opacity-20" />
            <div className="text-center">
              <p className="text-white text-2xl font-bold">4</p>
              <p className="text-blue-300 text-sm">Health Metrics</p>
            </div>
          </div>
        </div>
      </div>

      {/* Right Panel - Login Form */}
      <div className="w-full lg:w-1/2 flex items-center justify-center p-8 bg-gradient-to-br from-slate-50 to-blue-50">
        <div className="w-full max-w-md fade-in-up">
          {/* Mobile logo */}
          <div className="lg:hidden text-center mb-8">
            <div className="w-16 h-16 bg-gradient-to-br from-indigo-500 to-purple-600 rounded-2xl flex items-center justify-center mx-auto mb-4 shadow-lg">
              <Heart size={32} className="text-white" fill="rgba(255,255,255,0.4)" />
            </div>
          </div>

          <div className="mb-8">
            <h2 className="text-3xl font-bold text-gray-900">Welcome back 👋</h2>
            <p className="text-gray-500 mt-2">Sign in to view your health dashboard</p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-5">
            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">Email Address</label>
              <div className="relative">
                <Mail className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400" size={18} />
                <input
                  id="login-email"
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="health-input"
                  placeholder="you@example.com"
                  required
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">Password</label>
              <div className="relative">
                <Lock className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400" size={18} />
                <input
                  id="login-password"
                  type="password"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  className="health-input"
                  placeholder="••••••••"
                  required
                />
              </div>
            </div>

            <button id="login-btn" type="submit" disabled={loading} className="btn-primary">
              {loading ? (
                <span className="flex items-center justify-center gap-2">
                  <svg className="animate-spin h-4 w-4" viewBox="0 0 24 24" fill="none">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"/>
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"/>
                  </svg>
                  Signing in...
                </span>
              ) : 'Sign In →'}
            </button>
          </form>

          <p className="text-center text-gray-500 mt-6 text-sm">
            Don't have an account?{' '}
            <Link to="/register" className="text-indigo-600 hover:text-indigo-700 font-semibold">
              Create one free
            </Link>
          </p>

        </div>
      </div>
    </div>
  )
}