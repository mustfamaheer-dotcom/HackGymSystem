import { useQuery } from '@tanstack/react-query';
import { dashboardApi } from '../api/dashboard';
import { Card, CardContent } from '../components/ui/Card';
import { Spinner } from '../components/ui/Spinner';

export default function Dashboard() {
  const { data, isLoading } = useQuery({
    queryKey: ['dashboard'],
    queryFn: () => dashboardApi.getStats().then((r) => r.data.data),
  });

  if (isLoading) return <div className="flex justify-center py-20"><Spinner size="lg" /></div>;

  const stats = [
    { label: 'Total Members', value: data?.totalMembers ?? 0, color: 'bg-blue-500' },
    { label: 'Active Members', value: data?.activeMembers ?? 0, color: 'bg-green-500' },
    { label: 'Active Memberships', value: data?.activeMemberships ?? 0, color: 'bg-indigo-500' },
    { label: "Today's Check-ins", value: data?.todayCheckIns ?? 0, color: 'bg-cyan-500' },
    { label: 'Monthly Revenue', value: `EGP ${data?.monthlyRevenue?.toFixed(2) ?? '0.00'}`, color: 'bg-emerald-500' },
    { label: 'Pending Payments', value: data?.overduePayments ?? 0, color: 'bg-orange-500' },
    { label: 'Expiring Memberships', value: data?.expiringMemberships ?? 0, color: 'bg-red-500' },
  ];

  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-bold text-gray-900">Dashboard</h1>
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
        {stats.map((stat: any) => (
          <Card key={stat.label}>
            <CardContent className="p-5">
              <div className="flex items-center gap-3">
                <div className={`w-3 h-3 rounded-full ${stat.color}`} />
                <p className="text-sm text-gray-500">{stat.label}</p>
              </div>
              <p className="text-2xl font-bold text-gray-900 mt-2">{stat.value}</p>
            </CardContent>
          </Card>
        ))}
      </div>

      {data?.recentActivities && data.recentActivities.length > 0 && (
        <Card>
          <CardContent className="p-5">
            <h2 className="text-lg font-semibold text-gray-900 mb-4">Recent Activities</h2>
            <div className="space-y-3">
              {data.recentActivities.slice(0, 10).map((activity: any) => (
                <div key={activity.id} className="flex items-center justify-between text-sm">
                  <span className="text-gray-700">{activity.description}</span>
                  <span className="text-gray-400 text-xs">{new Date(activity.timestamp).toLocaleString()}</span>
                </div>
              ))}
            </div>
          </CardContent>
        </Card>
      )}
    </div>
  );
}
