import { useState, useRef, type FormEvent, type MouseEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

export default function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const from = '/dashboard';

  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false);
  const [rememberMe, setRememberMe] = useState(false);

  const btnRef = useRef<HTMLButtonElement>(null);

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);
    try {
      await login({ username, password });
      navigate(from, { replace: true });
    } catch (err: any) {
      setError(err.response?.data?.message || 'Invalid credentials');
    } finally {
      setLoading(false);
    }
  };

  const handleRipple = (e: MouseEvent<HTMLButtonElement>) => {
    const btn = btnRef.current;
    if (!btn) return;
    const rect = btn.getBoundingClientRect();
    const ripple = document.createElement('span');
    const size = Math.max(rect.width, rect.height);
    ripple.style.width = ripple.style.height = `${size}px`;
    ripple.style.left = `${e.clientX - rect.left - size / 2}px`;
    ripple.style.top = `${e.clientY - rect.top - size / 2}px`;
    ripple.className = 'absolute rounded-full bg-white/40 animate-[ripple_0.6s_ease-out]';
    btn.appendChild(ripple);
    setTimeout(() => ripple.remove(), 600);
  };

  return (
    <div className="h-screen bg-gradient-to-br from-off-white via-white to-light-gray font-['Poppins',sans-serif] flex items-center justify-center p-4 md:p-6 relative overflow-hidden">
      <div className="absolute inset-0 pointer-events-none">
        <div className="absolute -top-40 -right-40 w-96 h-96 bg-gold/10 rounded-full blur-3xl" />
        <div className="absolute -bottom-40 -left-40 w-96 h-96 bg-amber/5 rounded-full blur-3xl" />
      </div>

      <div className="w-full max-w-[1200px] h-full max-h-[700px] flex flex-col lg:flex-row rounded-[32px] overflow-hidden shadow-[0_4px_60px_rgba(0,0,0,0.08)] relative z-10 animate-fade-slide-up bg-white">
        {/* ===== LEFT PANEL ===== */}
        <div className="hidden lg:flex w-[45%] relative h-full flex-col items-center justify-center p-12"
          style={{
            background: `
              radial-gradient(ellipse 100% 80% at 30% 0%, rgba(255,213,74,0.25) 0%, transparent 65%),
              radial-gradient(ellipse 80% 50% at 70% 100%, rgba(244,180,0,0.10) 0%, transparent 50%),
              linear-gradient(160deg, #FFFDF5 0%, #FFF8E7 30%, #FEF7E0 60%, #F5F0E8 100%)
            `
          }}>
          <div className="absolute inset-0 opacity-[0.03]" style={{
            backgroundImage: `
              repeating-linear-gradient(0deg, transparent, transparent 32px, rgba(244,180,0,0.12) 32px, rgba(244,180,0,0.12) 33px),
              repeating-linear-gradient(90deg, transparent, transparent 32px, rgba(244,180,0,0.12) 32px, rgba(244,180,0,0.12) 33px)
            `
          }} />

          <div className="relative z-10 flex flex-col items-center text-center">
            <img
              src="/logo.png"
              alt="Hack Gym"
              className="w-32 h-32 xl:w-40 xl:h-40 object-contain mb-8 drop-shadow-xl"
            />

            <h1 className="text-4xl xl:text-5xl font-extrabold leading-[1.15] mb-4">
              <span className="text-charcoal">STRONGER</span><br />
              <span className="bg-gradient-to-r from-amber to-gold bg-clip-text text-transparent">EVERY DAY</span>
            </h1>

            <p className="text-soft-gray/80 text-sm leading-relaxed max-w-sm mx-auto mb-10">
              The all-in-one platform to manage your members, workouts, subscriptions, 
              trainers, and grow your fitness business seamlessly.
            </p>

            <div className="grid grid-cols-3 gap-3 w-full max-w-md">
              {[
                {
                  icon: (
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#F4B400" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
                      <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2" />
                      <circle cx="9" cy="7" r="4" />
                      <path d="M23 21v-2a4 4 0 0 0-3-3.87" />
                      <path d="M16 3.13a4 4 0 0 1 0 7.75" />
                    </svg>
                  ),
                  label: 'Manage Members'
                },
                {
                  icon: (
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#F4B400" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
                      <path d="M6.5 6.5 17.5 17.5" />
                      <path d="M6.5 17.5 17.5 6.5" />
                      <circle cx="12" cy="12" r="10" />
                    </svg>
                  ),
                  label: 'Track Workouts'
                },
                {
                  icon: (
                    <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="#F4B400" strokeWidth="1.8" strokeLinecap="round" strokeLinejoin="round">
                      <polyline points="23 6 13.5 15.5 8.5 10.5 1 18" />
                      <polyline points="17 6 23 6 23 12" />
                    </svg>
                  ),
                  label: 'Grow Business'
                }
              ].map((item, i) => (
                <div key={i} className="flex flex-col items-center gap-2 p-3 rounded-2xl bg-white/70 backdrop-blur-sm border border-gold/20 shadow-sm hover:shadow-md hover:border-gold/40 transition-all duration-300">
                  <div className="w-9 h-9 rounded-xl bg-gradient-to-br from-amber/10 to-gold/10 flex items-center justify-center">
                    {item.icon}
                  </div>
                  <span className="text-charcoal/70 text-[10px] font-semibold leading-tight text-center">{item.label}</span>
                </div>
              ))}
            </div>
          </div>
        </div>

        {/* ===== RIGHT PANEL ===== */}
        <div className="w-full lg:w-[55%] bg-white p-8 md:p-10 lg:p-14 relative flex items-center justify-center">
          <div className="absolute inset-0 bg-gradient-to-br from-gold/[0.02] via-transparent to-amber/[0.02] pointer-events-none" />

          <div className="relative z-10 w-full max-w-[440px]">
            {/* Mobile logo */}
            <div className="flex lg:hidden items-center justify-center mb-6">
              <img src="/logo.png" alt="Hack Gym" className="w-10 h-10 object-contain" />
            </div>

            {/* Heading */}
            <div className="mb-8">
              <h2 className="text-3xl md:text-4xl font-bold text-charcoal mb-2">
                Welcome Back!
              </h2>
              <div className="w-14 h-1 bg-gradient-to-r from-amber to-gold rounded-full mb-4" />
              <p className="text-soft-gray/70 text-sm">Sign in to continue to Hack Gym System</p>
            </div>

            {/* Error */}
            {error && (
              <div className="mb-4 px-4 py-3 rounded-xl bg-red-50 border border-red-200 text-red-600 text-sm flex items-center gap-2 animate-fade-slide-up">
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <circle cx="12" cy="12" r="10" />
                  <line x1="15" y1="9" x2="9" y2="15" />
                  <line x1="9" y1="9" x2="15" y2="15" />
                </svg>
                {error}
              </div>
            )}

            {/* Form */}
            <form onSubmit={handleSubmit}>
              <div className="space-y-5">
                {/* Username */}
                <div className="space-y-1.5">
                  <label htmlFor="username" className="text-charcoal text-sm font-medium block">
                    Username
                  </label>
                  <div className="relative group">
                    <svg className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-soft-gray/50 group-focus-within:text-amber transition-colors duration-200" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                      <circle cx="12" cy="8" r="4" />
                      <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2" />
                    </svg>
                    <input
                      id="username"
                      type="text"
                      value={username}
                      onChange={(e) => setUsername(e.target.value)}
                      placeholder="Enter your username"
                      required
                      className="w-full bg-light-gray border border-gray-200/80 text-charcoal placeholder-soft-gray/50 rounded-xl pl-10 pr-4 py-3.5 text-sm
                        focus:outline-none focus:border-amber/40 focus:ring-2 focus:ring-amber/15
                        transition-all duration-200"
                    />
                  </div>
                </div>

                {/* Password */}
                <div className="space-y-1.5">
                  <label htmlFor="password" className="text-charcoal text-sm font-medium block">
                    Password
                  </label>
                  <div className="relative group">
                    <svg className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-soft-gray/50 group-focus-within:text-amber transition-colors duration-200" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                      <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                      <path d="M7 11V7a5 5 0 0 1 10 0v4" />
                    </svg>
                    <input
                      id="password"
                      type={showPassword ? 'text' : 'password'}
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                      placeholder="Enter your password"
                      required
                      className="w-full bg-light-gray border border-gray-200/80 text-charcoal placeholder-soft-gray/50 rounded-xl pl-10 pr-10 py-3.5 text-sm
                        focus:outline-none focus:border-amber/40 focus:ring-2 focus:ring-amber/15
                        transition-all duration-200"
                    />
                    <button
                      type="button"
                      onClick={() => setShowPassword(!showPassword)}
                      tabIndex={-1}
                      className="absolute right-3.5 top-1/2 -translate-y-1/2 text-soft-gray/50 hover:text-charcoal/70 transition-colors duration-200"
                    >
                      {showPassword ? (
                        <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                          <path d="M17.94 17.94A10.07 10.07 0 0 1 12 20c-7 0-11-8-11-8a18.45 18.45 0 0 1 5.06-5.94" />
                          <path d="M9.9 4.24A9.12 9.12 0 0 1 12 4c7 0 11 8 11 8a18.5 18.5 0 0 1-2.16 3.19" />
                          <line x1="1" y1="1" x2="23" y2="23" />
                        </svg>
                      ) : (
                        <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                          <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z" />
                          <circle cx="12" cy="12" r="3" />
                        </svg>
                      )}
                    </button>
                  </div>
                </div>

                {/* Remember Me */}
                <div className="flex items-center gap-2.5">
                  <button
                    type="button"
                    onClick={() => setRememberMe(!rememberMe)}
                    className={`w-4.5 h-4.5 rounded-[5px] border flex items-center justify-center transition-all duration-200 ${
                      rememberMe
                        ? 'bg-gradient-to-r from-amber to-gold border-amber'
                        : 'bg-light-gray border-gray-200/80 hover:border-amber/40'
                    }`}
                  >
                    {rememberMe && (
                      <svg width="10" height="10" viewBox="0 0 24 24" fill="none" stroke="white" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round">
                        <polyline points="20 6 9 17 4 12" />
                      </svg>
                    )}
                  </button>
                  <label onClick={() => setRememberMe(!rememberMe)} className="text-soft-gray/70 text-sm cursor-pointer select-none">
                    Remember me
                  </label>
                </div>

                {/* Mobile feature badges */}
                <div className="flex lg:hidden flex-wrap gap-2 pt-1">
                  {['Manage Members', 'Track Workouts', 'Grow Business'].map((label, i) => (
                    <span key={i} className="text-[10px] text-soft-gray/60 bg-light-gray border border-gray-200/60 px-2.5 py-1 rounded-full">
                      {label}
                    </span>
                  ))}
                </div>
              </div>

              {/* Sign In Button */}
              <div className="mt-7">
                <button
                  ref={btnRef}
                  type="submit"
                  disabled={loading}
                  onClick={handleRipple}
                  className="relative overflow-hidden w-full bg-gradient-to-r from-amber to-gold text-white font-semibold text-sm rounded-xl py-3.5
                    flex items-center justify-center gap-2
                    shadow-[0_4px_20px_rgba(244,180,0,0.25)] hover:shadow-[0_8px_30px_rgba(244,180,0,0.35)]
                    hover:-translate-y-0.5 active:scale-[0.98]
                    disabled:opacity-60 disabled:cursor-not-allowed disabled:hover:shadow-[0_4px_20px_rgba(244,180,0,0.25)] disabled:hover:translate-y-0 disabled:active:scale-100
                    transition-all duration-200"
                >
                  {loading ? (
                    <svg className="animate-spin h-5 w-5" viewBox="0 0 24 24" fill="none">
                      <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                      <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
                    </svg>
                  ) : (
                    <>
                      Sign In
                      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round">
                        <line x1="5" y1="12" x2="19" y2="12" />
                        <polyline points="12 5 19 12 12 19" />
                      </svg>
                    </>
                  )}
                </button>
              </div>
            </form>

            {/* Security Message */}
            <div className="flex items-center justify-center gap-2 mt-6 text-soft-gray/50 text-xs">
              <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                <rect x="3" y="11" width="18" height="11" rx="2" ry="2" />
                <path d="M7 11V7a5 5 0 0 1 10 0v4" />
              </svg>
              Your data is 100% secure and encrypted
            </div>
          </div>
        </div>
      </div>

    </div>
  );
}
