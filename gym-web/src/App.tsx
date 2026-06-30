import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
import { AuthProvider } from './contexts/AuthContext';
import { ProtectedRoute } from './components/layout/ProtectedRoute';
import { AppLayout } from './components/layout/AppLayout';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import MemberList from './pages/members/MemberList';
import PlanList from './pages/plans/PlanList';
import MembershipList from './pages/memberships/MembershipList';
import AttendanceList from './pages/attendance/AttendanceList';
import PaymentList from './pages/payments/PaymentList';
import OfferList from './pages/offers/OfferList';
import DeviceList from './pages/devices/DeviceList';
import NotificationList from './pages/notifications/NotificationList';
import SettingsPage from './pages/settings/SettingsPage';
import UserList from './pages/users/UserList';
import NotFound from './pages/NotFound';

export default function App() {
  return (
    <BrowserRouter>
      <AuthProvider>
        <Toaster position="top-right" toastOptions={{ duration: 4000 }} />
        <Routes>
          <Route path="/login" element={<Login />} />
          <Route
            element={
              <ProtectedRoute>
                <AppLayout />
              </ProtectedRoute>
            }
          >
            <Route path="/dashboard" element={<Dashboard />} />
            <Route path="/members" element={<MemberList />} />
            <Route path="/plans" element={<PlanList />} />
            <Route path="/memberships" element={<MembershipList />} />
            <Route path="/attendance" element={<AttendanceList />} />
            <Route path="/payments" element={<PaymentList />} />
            <Route path="/offers" element={<OfferList />} />
            <Route path="/devices" element={<DeviceList />} />
            <Route path="/notifications" element={<NotificationList />} />
            <Route path="/users" element={<UserList />} />
            <Route path="/settings" element={<SettingsPage />} />
            <Route path="/" element={<Navigate to="/dashboard" replace />} />
            <Route path="*" element={<NotFound />} />
          </Route>
        </Routes>
      </AuthProvider>
    </BrowserRouter>
  );
}
