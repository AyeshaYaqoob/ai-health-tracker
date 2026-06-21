import { useState } from 'react'
import { Outlet, NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'
import { useQuery } from '@tanstack/react-query'
import { getAlerts } from '../../api/logs'
import {
  LayoutDashboard,
  ClipboardList,
  Brain,
  FileText,
  LogOut,
  Heart,
  Activity,
  Bell,
  UserCircle,
  Menu,
  X,
} from 'lucide-react'

export default function Layout() {
  const { user, logout } = useAuth()
  const navigate = useNavigate()
  const [sidebarOpen, setSidebarOpen] = useState(false)

  // Fetch alerts count for badge
  const { data: alertsData } = useQuery({
    queryKey: ['alerts'],
    queryFn: getAlerts,
    staleTime: 1000 * 60 * 5,
    retry: 1,
  })
  const alertCount = alertsData?.alerts.filter(a => a.severity === 'danger' || a.severity === 'warning').length ?? 0

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  const navLinks = [
    { to: '/', label: 'Dashboard', icon: LayoutDashboard, end: true },
    { to: '/logs', label: 'Daily Logs', icon: ClipboardList, end: false },
    { to: '/insights', label: 'AI Insights', icon: Brain, end: false },
    { to: '/reports', label: 'Weekly Reports', icon: FileText, end: false },
    { to: '/alerts', label: 'Health Alerts', icon: Bell, end: false, badge: alertCount },
  ]

  const SidebarContent = () => (
    <>
      {/* Logo */}
      <div className="p-6 pb-5">
        <div className="flex items-center gap-3 mb-1">
          <div className="w-9 h-9 bg-gradient-to-br from-rose-400 to-pink-600 rounded-xl flex items-center justify-center shadow-lg">
            <Heart size={18} className="text-white" fill="rgba(255,255,255,0.5)" />
          </div>
          <span className="font-bold text-lg text-white tracking-tight">HealthTracker</span>
        </div>
        <p className="text-xs text-blue-300 ml-12 opacity-70">AI-Powered</p>
      </div>

      {/* User info */}
      <div className="mx-4 mb-5 p-4 rounded-2xl" style={{ background: 'rgba(255,255,255,0.07)', border: '1px solid rgba(255,255,255,0.1)' }}>
        <div className="flex items-center gap-3">
          <div className="w-10 h-10 bg-gradient-to-br from-indigo-400 to-purple-500 rounded-full flex items-center justify-center text-white font-bold text-sm shadow-md">
            {user?.firstName?.[0]}{user?.lastName?.[0]}
          </div>
          <div>
            <p className="font-semibold text-white text-sm">{user?.firstName} {user?.lastName}</p>
            <div className="flex items-center gap-1.5 mt-0.5">
              <div className="pulse-dot" style={{ width: 7, height: 7 }}></div>
              <p className="text-xs text-emerald-400">Active</p>
            </div>
          </div>
        </div>
      </div>

      {/* Navigation */}
      <nav className="flex-1 px-3 space-y-1">
        <p className="text-xs font-semibold text-blue-400 opacity-60 uppercase tracking-widest px-3 mb-3">Menu</p>

        {navLinks.map(({ to, label, icon: Icon, end, badge }) => (
          <NavLink
            key={to}
            to={to}
            end={end}
            onClick={() => setSidebarOpen(false)}
            className={({ isActive }) =>
              `flex items-center gap-3 px-4 py-3 rounded-xl transition-all text-sm font-medium relative ${
                isActive ? 'nav-active' : 'text-blue-200 hover:text-white hover:bg-white hover:bg-opacity-5'
              }`
            }
          >
            <Icon size={18} />
            {label}
            {badge !== undefined && badge > 0 && (
              <span className="ml-auto px-2 py-0.5 rounded-full text-xs font-bold bg-red-500 text-white">
                {badge}
              </span>
            )}
          </NavLink>
        ))}
      </nav>

      {/* Bottom */}
      <div className="p-4 border-t border-white border-opacity-5">
        <div className="flex items-center gap-2 px-3 mb-2">
          <Activity size={14} className="text-blue-400" />
          <span className="text-xs text-blue-300 opacity-60">30 days of data tracked</span>
        </div>

        {/* Profile link */}
        <NavLink
          to="/profile"
          onClick={() => setSidebarOpen(false)}
          className={({ isActive }) =>
            `flex items-center gap-3 px-4 py-2.5 rounded-xl transition-all w-full text-sm font-medium mb-1 ${
              isActive ? 'nav-active' : 'text-blue-200 hover:text-white hover:bg-white hover:bg-opacity-5'
            }`
          }
        >
          <UserCircle size={18} />
          Profile
        </NavLink>

        <button
          onClick={handleLogout}
          className="flex items-center gap-3 px-4 py-2.5 rounded-xl text-blue-300 hover:text-rose-400 hover:bg-rose-500 hover:bg-opacity-10 transition-all w-full text-sm font-medium"
        >
          <LogOut size={18} />
          Sign Out
        </button>
      </div>
    </>
  )

  return (
    <div className="flex h-screen" style={{ background: '#f0f4ff' }}>
      {/* Desktop Sidebar */}
      <div className="hidden md:flex w-64 sidebar-gradient flex-col shadow-2xl flex-shrink-0">
        <SidebarContent />
      </div>

      {/* Mobile sidebar overlay */}
      {sidebarOpen && (
        <div
          className="fixed inset-0 z-40 bg-black bg-opacity-50 md:hidden"
          onClick={() => setSidebarOpen(false)}
        />
      )}

      {/* Mobile sidebar drawer */}
      <div className={`fixed inset-y-0 left-0 z-50 w-64 sidebar-gradient flex-col shadow-2xl flex md:hidden transform transition-transform duration-300 ${
        sidebarOpen ? 'translate-x-0' : '-translate-x-full'
      }`}>
        <button
          onClick={() => setSidebarOpen(false)}
          className="absolute top-4 right-4 p-2 rounded-lg text-blue-300 hover:text-white hover:bg-white hover:bg-opacity-10 transition-all"
        >
          <X size={18} />
        </button>
        <SidebarContent />
      </div>

      {/* Main content */}
      <div className="flex-1 overflow-auto dashboard-bg flex flex-col">
        {/* Mobile top bar */}
        <div className="md:hidden flex items-center gap-3 px-4 py-3 bg-white border-b border-gray-100 shadow-sm sticky top-0 z-30">
          <button
            onClick={() => setSidebarOpen(true)}
            className="p-2 rounded-lg text-gray-600 hover:bg-gray-100 transition-all"
          >
            <Menu size={20} />
          </button>
          <div className="flex items-center gap-2">
            <div className="w-7 h-7 bg-gradient-to-br from-rose-400 to-pink-600 rounded-lg flex items-center justify-center">
              <Heart size={14} className="text-white" fill="rgba(255,255,255,0.5)" />
            </div>
            <span className="font-bold text-gray-900">HealthTracker</span>
          </div>
          {alertCount > 0 && (
            <NavLink to="/alerts" className="ml-auto relative">
              <Bell size={20} className="text-gray-500" />
              <span className="absolute -top-1 -right-1 w-4 h-4 rounded-full bg-red-500 text-white text-xs flex items-center justify-center font-bold">
                {alertCount}
              </span>
            </NavLink>
          )}
        </div>

        <div className="fade-in-up flex-1">
          <Outlet />
        </div>
      </div>
    </div>
  )
}