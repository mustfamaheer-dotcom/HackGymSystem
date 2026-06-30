import { useState } from 'react';
import { NavLink, Outlet, useNavigate } from 'react-router-dom';
import { useAuth } from '../../hooks/useAuth';
import { cn } from '../../lib/utils';

const navItems = [
  { to: '/dashboard', label: 'Dashboard', icon: '📊' },
  { to: '/members', label: 'Members', icon: '👥' },
  { to: '/plans', label: 'Plans', icon: '📋', adminOnly: true },
  { to: '/memberships', label: 'Memberships', icon: '🎫' },
  { to: '/attendance', label: 'Attendance', icon: '✅' },
  { to: '/payments', label: 'Payments', icon: '💰' },
  { to: '/offers', label: 'Offers', icon: '🏷️', adminOnly: true },
  { to: '/devices', label: 'Devices', icon: '🖥️', adminOnly: true },
  { to: '/notifications', label: 'Notifications', icon: '🔔' },
  { to: '/users', label: 'Users', icon: '🔐', adminOnly: true },
  { to: '/settings', label: 'Settings', icon: '⚙️', adminOnly: true },
];

export function AppLayout() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [sidebarOpen, setSidebarOpen] = useState(false);

  const handleLogout = async () => {
    await logout();
    navigate('/login');
  };

  return (
    <div className="flex h-screen bg-gradient-to-br from-gray-50 to-gray-100">
      <aside className={cn(
        'fixed inset-y-0 left-0 z-30 w-64 bg-white/80 backdrop-blur-xl border-r border-gray-200/60 transform transition-all duration-300 ease-out lg:translate-x-0 lg:static lg:inset-auto',
        sidebarOpen ? 'translate-x-0 shadow-2xl' : '-translate-x-full'
      )}>
        <div className="flex items-center gap-3 h-16 px-5 border-b border-gray-200/60">
          <img src="/logo.png" alt="Logo" className="h-9 w-9 rounded-xl shadow-sm" />
          <h1 className="text-lg font-bold bg-gradient-to-r from-indigo-600 to-purple-600 bg-clip-text text-transparent">Hack Gym</h1>
        </div>
        <nav className="p-3 space-y-1">
          {navItems
            .filter((item) => !item.adminOnly || user?.role === 'Owner')
            .map(({ to, label, icon }) => (
            <NavLink
              key={to}
              to={to}
              onClick={() => setSidebarOpen(false)}
              className={({ isActive }) => cn(
                'flex items-center px-3 py-2.5 rounded-xl text-sm font-medium transition-all duration-200 ease-out',
                isActive
                  ? 'bg-gradient-to-r from-indigo-50 to-purple-50 text-indigo-700 shadow-sm scale-[1.02]'
                  : 'text-gray-500 hover:bg-gray-100/70 hover:text-gray-800 hover:scale-[1.01]'
              )}
            >
              <span className="mr-3 text-lg">{icon}</span>
              {label}
            </NavLink>
          ))}
        </nav>
      </aside>

      {sidebarOpen && (
        <div className="fixed inset-0 z-20 bg-black/30 backdrop-blur-sm lg:hidden transition-all duration-300" onClick={() => setSidebarOpen(false)} />
      )}

      <div className="flex-1 flex flex-col min-w-0">
        <header className="h-16 bg-white/80 backdrop-blur-xl border-b border-gray-200/60 flex items-center justify-between px-4 lg:px-6 sticky top-0 z-10">
          <button className="lg:hidden p-2 text-gray-500 hover:text-gray-700 hover:bg-gray-100 rounded-lg transition-all duration-200" onClick={() => setSidebarOpen(true)}>
            <svg className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 12h16M4 18h16" />
            </svg>
          </button>
          <div className="flex items-center gap-4 ml-auto">
            <span className="text-sm text-gray-600">{user?.fullName || user?.username}</span>
            <button onClick={handleLogout} className="text-sm text-gray-400 hover:text-red-500 transition-all duration-200 hover:scale-105">
              Logout
            </button>
          </div>
        </header>
        <main className="flex-1 overflow-auto p-4 lg:p-6">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
