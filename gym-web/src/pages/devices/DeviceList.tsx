import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { devicesApi } from '../../api/devices';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { Modal } from '../../components/ui/Modal';
import { Table } from '../../components/ui/Table';
import { Card, CardContent } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';
import type { DeviceDto } from '../../types';

export default function DeviceList() {
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<DeviceDto | null>(null);
  const [logsModal, setLogsModal] = useState<string | null>(null);
  const [form, setForm] = useState({ name: '', serialNumber: '', deviceType: 'Fingerprint', ipAddress: '', port: 4370, location: '', notes: '' });
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery({
    queryKey: ['devices'],
    queryFn: () => devicesApi.getAll().then((r) => r.data.data),
  });

  const { data: logs } = useQuery({
    queryKey: ['device-logs', logsModal],
    queryFn: () => devicesApi.getLogs(logsModal!).then((r) => r.data.data),
    enabled: !!logsModal,
  });

  const createMutation = useMutation({
    mutationFn: () => devicesApi.create(form),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['devices'] }); closeModal(); },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data: d }: { id: string; data: any }) => devicesApi.update(id, d),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['devices'] }); closeModal(); },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => devicesApi.delete(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['devices'] }),
  });

  const syncMutation = useMutation({
    mutationFn: (id: string) => devicesApi.sync(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['devices'] }),
  });

  const openCreate = () => {
    setEditing(null);
    setForm({ name: '', serialNumber: '', deviceType: 'Fingerprint', ipAddress: '', port: 4370, location: '', notes: '' });
    setModalOpen(true);
  };

  const openEdit = (d: DeviceDto) => {
    setEditing(d);
    setForm({ name: d.name, serialNumber: d.serialNumber, deviceType: d.deviceType, ipAddress: d.ipAddress || '', port: d.port || 4370, location: d.location || '', notes: d.notes || '' });
    setModalOpen(true);
  };

  const closeModal = () => { setModalOpen(false); setEditing(null); };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (editing) updateMutation.mutate({ id: editing.id, data: form });
    else createMutation.mutate();
  };

  const devices = data || [];

  const columns = [
    { header: 'Name', accessor: 'name' as keyof DeviceDto },
    { header: 'Serial', accessor: 'serialNumber' as keyof DeviceDto },
    { header: 'Type', accessor: 'deviceType' as keyof DeviceDto },
    { header: 'IP', accessor: (d: DeviceDto) => d.ipAddress || '-' },
    { header: 'Location', accessor: (d: DeviceDto) => d.location || '-' },
    { header: 'Last Sync', accessor: (d: DeviceDto) => d.lastSync ? new Date(d.lastSync).toLocaleString() : '-' },
    { header: 'Status', accessor: (d: DeviceDto) => <Badge>{d.isActive ? 'Online' : 'Offline'}</Badge> },
    { header: 'Actions', accessor: (d: DeviceDto) => (
      <div className="flex gap-1" onClick={(e) => e.stopPropagation()}>
        <Button size="sm" variant="ghost" onClick={() => openEdit(d)}>Edit</Button>
        <Button size="sm" variant="ghost" onClick={() => syncMutation.mutate(d.id)}>Sync</Button>
        <Button size="sm" variant="ghost" onClick={() => setLogsModal(d.id)}>Logs</Button>
        <Button size="sm" variant="danger" onClick={() => { if (confirm('Delete device?')) deleteMutation.mutate(d.id); }}>Delete</Button>
      </div>
    ) as any },
  ];

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-900">Devices</h1>
        <Button onClick={openCreate}>Add Device</Button>
      </div>
      <Card>
        <CardContent className="p-0">
          <Table columns={columns} data={devices} loading={isLoading} keyExtractor={(d) => d.id} />
        </CardContent>
      </Card>

      <Modal open={modalOpen} onClose={closeModal} title={editing ? 'Edit Device' : 'Add Device'}>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div className="grid grid-cols-2 gap-4">
            <Input label="Device Name" id="name" value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} required />
            <Input label="Serial Number" id="serialNumber" value={form.serialNumber} onChange={(e) => setForm({ ...form, serialNumber: e.target.value })} required />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <Input label="Device Type" id="deviceType" value={form.deviceType} onChange={(e) => setForm({ ...form, deviceType: e.target.value })} required />
            <Input label="IP Address" id="ipAddress" value={form.ipAddress} onChange={(e) => setForm({ ...form, ipAddress: e.target.value })} />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <Input label="Port" id="port" type="number" value={form.port} onChange={(e) => setForm({ ...form, port: Number(e.target.value) })} />
            <Input label="Location" id="location" value={form.location} onChange={(e) => setForm({ ...form, location: e.target.value })} />
          </div>
          <Input label="Notes" id="notes" value={form.notes} onChange={(e) => setForm({ ...form, notes: e.target.value })} />
          <div className="flex justify-end gap-3">
            <Button type="button" variant="secondary" onClick={closeModal}>Cancel</Button>
            <Button type="submit" loading={createMutation.isPending || updateMutation.isPending}>{editing ? 'Update' : 'Create'}</Button>
          </div>
        </form>
      </Modal>

      <Modal open={!!logsModal} onClose={() => setLogsModal(null)} title="Device Logs" size="lg">
        {logs && logs.length > 0 ? (
          <div className="max-h-96 overflow-y-auto space-y-2">
            {logs.map((log: any) => (
              <div key={log.id} className="flex items-start justify-between text-sm border-b border-gray-100 pb-2">
                <div><span className="font-medium">{log.logType}</span>: {log.message}</div>
                <span className="text-gray-400 text-xs whitespace-nowrap ml-4">{new Date(log.timestamp).toLocaleString()}</span>
              </div>
            ))}
          </div>
        ) : (
          <p className="text-gray-500 text-center py-8">No logs found</p>
        )}
      </Modal>
    </div>
  );
}
