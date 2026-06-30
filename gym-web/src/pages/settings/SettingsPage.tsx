import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { settingsApi } from '../../api/settings';
import { devicesApi } from '../../api/devices';
import { Button } from '../../components/ui/Button';
import { Card, CardContent, CardHeader } from '../../components/ui/Card';
import { Spinner } from '../../components/ui/Spinner';
import { useState } from 'react';
import type { SettingDto } from '../../types';

export default function SettingsPage() {
  const queryClient = useQueryClient();
  const [editingId, setEditingId] = useState<string | null>(null);
  const [editValue, setEditValue] = useState('');
  const [testDeviceId, setTestDeviceId] = useState('');
  const [testResult, setTestResult] = useState<{ success: boolean; message: string } | null>(null);

  const { data, isLoading } = useQuery({
    queryKey: ['settings'],
    queryFn: () => settingsApi.getAll().then((r) => r.data.data),
  });

  const { data: devices } = useQuery({
    queryKey: ['devices-active'],
    queryFn: () => devicesApi.getActive().then((r) => r.data.data),
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, value }: { id: string; value: string }) => settingsApi.update(id, value),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['settings'] }); setEditingId(null); },
  });

  const testMutation = useMutation({
    mutationFn: (id: string) => devicesApi.testConnection(id),
    onSuccess: (res) => setTestResult(res.data.data),
    onError: () => setTestResult({ success: false, message: 'Failed to reach server' }),
  });

  const settings = data || [];
  const deviceList = devices || [];

  const grouped = settings.reduce<Record<string, SettingDto[]>>((acc, s) => {
    const group = s.group || 'General';
    (acc[group] = acc[group] || []).push(s);
    return acc;
  }, {});

  if (isLoading) return <div className="flex justify-center py-20"><Spinner size="lg" /></div>;

  return (
    <div className="space-y-6">
      <h1 className="text-2xl font-bold text-gray-900">Settings</h1>

      <Card>
        <CardHeader><h2 className="text-lg font-semibold text-gray-900">Device Connection Test</h2></CardHeader>
        <CardContent className="space-y-4">
          <p className="text-sm text-gray-600">Test connectivity between the system and your ZKTeco device.</p>
          <div className="flex items-end gap-3">
            <div className="flex-1">
              <label className="block text-sm font-medium text-gray-700 mb-1">Select Device</label>
              <select
                value={testDeviceId}
                onChange={(e) => { setTestDeviceId(e.target.value); setTestResult(null); }}
                className="block w-full rounded-lg border border-gray-300 px-3 py-2 text-sm shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500"
              >
                <option value="">-- Select a device --</option>
                {deviceList.map((d: any) => (
                  <option key={d.id} value={d.id}>{d.name} ({d.ipAddress || d.iPAddress || 'No IP'}:{d.port || 4370})</option>
                ))}
              </select>
            </div>
            <Button onClick={() => testMutation.mutate(testDeviceId)} disabled={!testDeviceId} loading={testMutation.isPending}>
              Test Connection
            </Button>
          </div>
          {testResult && (
            <div className={`flex items-center gap-2 p-3 rounded-lg text-sm ${testResult.success ? 'bg-green-50 text-green-800' : 'bg-red-50 text-red-800'}`}>
              <span className="text-lg">{testResult.success ? '✓' : '✗'}</span>
              <span>{testResult.message}</span>
            </div>
          )}
          {deviceList.length === 0 && (
            <p className="text-sm text-amber-600">No active devices found. Add a device in the Devices page first.</p>
          )}
        </CardContent>
      </Card>

      {Object.entries(grouped).map(([group, items]) => (
        <Card key={group}>
          <CardHeader><h2 className="text-lg font-semibold text-gray-900 capitalize">{group}</h2></CardHeader>
          <CardContent className="space-y-4">
            {items.map((setting) => (
              <div key={setting.id} className="flex items-center justify-between">
                <div>
                  <p className="text-sm font-medium text-gray-900">{setting.key}</p>
                  {setting.description && <p className="text-xs text-gray-500">{setting.description}</p>}
                </div>
                <div className="flex items-center gap-2">
                  {editingId === setting.id ? (
                    <>
                      <input type="text" value={editValue} onChange={(e) => setEditValue(e.target.value)} className="border border-gray-300 rounded-lg px-3 py-1.5 text-sm focus:border-indigo-500 focus:outline-none" autoFocus />
                      <Button size="sm" onClick={() => updateMutation.mutate({ id: setting.id, value: editValue })} loading={updateMutation.isPending}>Save</Button>
                      <Button size="sm" variant="ghost" onClick={() => setEditingId(null)}>Cancel</Button>
                    </>
                  ) : (
                    <>
                      <span className="text-sm text-gray-700 font-mono">{setting.value}</span>
                      <Button size="sm" variant="ghost" onClick={() => { setEditingId(setting.id); setEditValue(setting.value); }}>Edit</Button>
                    </>
                  )}
                </div>
              </div>
            ))}
          </CardContent>
        </Card>
      ))}
    </div>
  );
}
