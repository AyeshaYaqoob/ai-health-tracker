import { useState } from 'react'
import { useNavigate, Link } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'
import { register as registerApi } from '../api/auth'
import toast from 'react-hot-toast'
import { Heart, Mail, Lock, User, Activity, Moon, Brain, Shield } from 'lucide-react'
import axios from 'axios'

export default function RegisterPage() {
  const [form, setForm] = useState({ firstName: '', lastName: '', email: '', password: '' })
  const [loading, setLoading] = useState(false)
  const { login } = useAuth()
  const navigate = useNavigate()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    try {
      const data = await registerApi(form)
      login(data)
      toast.success(`Welcome, ${data.firstName}! Let's track your health 🎉`)
      navigate('/')
    } catch (err) {
      if (axios.isAxiosError(err)) {
        if (!err.response) toast.error('Cannot connect to server. Make sure the backend is running.')
        else if (err.response.status === 409 || err.response.status === 400)
          toast.error(err.response.data?.message || 'Email already registered.')
        else toast.error(`Registration failed: ${err.response.data?.message || err.response.statusText}`)
      } else {
        toast.error('An unexpected error occurred. Please try again.')
      }
    } finally {
      setLoading(false)
    }
  }

  const features = [
    { icon: Activity, color: 'text-emerald-400', bg: 'bg-emerald-500', label: 'Track symptoms, mood & sleep daily' },
    { icon: Brain, color: 'text-purple-400', bg: 'bg-purple-500', label: 'AI-powered health pattern analysis' },
    { icon: Moon, color: 'text-blue-400', bg: 'bg-blue-500', label: 'Weekly AI-generated health reports' },
    { icon: Shield, color: 'text-rose-400', bg: 'bg-rose-500', label: 'Secure & private health data' },
  ]

  return (
    <div className="min-h-screen flex">
      {/* Left Panel */}
      <div className="hidden lg:flex lg:w-1/2 auth-bg flex-col items-center justify-center p-12 relative">
        <div className="z-10 text-center max-w-sm">
          <div className="w-24 h-24 bg-white bg-opacity-10 rounded-3xl flex items-center justify-center mx-auto mb-6 backdrop-blur-sm border border-white border-opacity-20">
            <Heart size={48} className="text-rose-400" fill="rgba(251,113,133,0.4)" />
          </div>
          <h1 className="text-4xl font-bold text-white mb-3">Start Your Health Journey</h1>
          <p className="text-blue-200 text-lg leading-relaxed mb-10">
            Join thousands tracking their health with AI insights
          </p>

          <div className="space-y-4 text-left">
            {features.map(({ icon: Icon, color, bg, label }) => (
              <div key={label} className="flex items-center gap-4 glass-card px-5 py-4 rounded-2xl">
                <div className={`w-9 h-9 ${bg} bg-opacity-20 rounded-xl flex items-center justify-center flex-shrink-0`}>
                  <Icon size={18} className={color} />
                </div>
                <p className="text-white text-sm font-medium">{label}</p>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Right Panel - Form */}
      <div className="w-full lg:w-1/2 flex items-center justify-center p-8 bg-gradient-to-br from-slate-50 to-purple-50">
        <div className="w-full max-w-md fade-in-up">
          <div className="mb-8">
            <h2 className="text-3xl font-bold text-gray-900">Create Account ✨</h2>
            <p className="text-gray-500 mt-2">Start tracking your health today — it's free</p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">First Name</label>
                <div className="relative">
                  <User className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400" size={17} />
                  <input
                    type="text" value={form.firstName}
                    onChange={(e) => setForm({ ...form, firstName: e.target.value })}
                    className="health-input" placeholder="Zimal" required
                  />
                </div>
              </div>
              <div>
                <label className="block text-sm font-semibold text-gray-700 mb-2">Last Name</label>
                <div className="relative">
                  <User className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400" size={17} />
                  <input
                    type="text" value={form.lastName}
                    onChange={(e) => setForm({ ...form, lastName: e.target.value })}
                    className="health-input" placeholder="Hameed" required
                  />
                </div>
              </div>
            </div>

            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">Email Address</label>
              <div className="relative">
                <Mail className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400" size={17} />
                <input
                  type="email" value={form.email}
                  onChange={(e) => setForm({ ...form, email: e.target.value })}
                  className="health-input" placeholder="you@example.com" required
                />
              </div>
            </div>

            <div>
              <label className="block text-sm font-semibold text-gray-700 mb-2">Password</label>
              <div className="relative">
                <Lock className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-400" size={17} />
                <input
                  type="password" value={form.password}
                  onChange={(e) => setForm({ ...form, password: e.target.value })}
                  className="health-input" placeholder="Min. 8 characters" required minLength={6}
                />
              </div>
            </div>

            <button id="register-btn" type="submit" disabled={loading} className="btn-primary">
              {loading ? (
                <span className="flex items-center justify-center gap-2">
                  <svg className="animate-spin h-4 w-4" viewBox="0 0 24 24" fill="none">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
                  </svg>
                  Creating your account...
                </span>
              ) : 'Create Free Account →'}
            </button>
          </form>

          <p className="text-center text-gray-500 mt-6 text-sm">
            Already have an account?{' '}
            <Link to="/login" className="text-indigo-600 hover:text-indigo-700 font-semibold">Sign in</Link>
          </p>
        </div>
      </div>
    </div>
  )
}