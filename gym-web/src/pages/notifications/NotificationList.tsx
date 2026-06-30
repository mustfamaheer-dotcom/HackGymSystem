import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { notificationsApi } from '../../api/notifications';
import { membersApi } from '../../api/members';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { Select } from '../../components/ui/Select';
import { Modal } from '../../components/ui/Modal';
import { Table } from '../../components/ui/Table';
import { Card, CardContent, CardHeader } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';
import { formatDateTime } from '../../lib/utils';
import type { NotificationDto } from '../../types';

export default function NotificationList() {
  const [page, setPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [form, setForm] = useState({ memberId: '', type: 'Reminder', channel: 'WhatsApp', subject: '', message: '' });
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery({
    queryKey: ['notifications', page, statusFilter],
    queryFn: () => notificationsApi.getAll({ page, pageSize: 10, status: statusFilter || undefined }).then((r) => r.data.data),
  });

  const { data: membersData } = useQuery({ queryKey: ['members-small3'], queryFn: () => membersApi.getAll({ pageSize: 100 }).then((r) => r.data.data) });

  const sendMutation = useMutation({
    mutationFn: () => notificationsApi.send({ memberId: form.memberId, type: form.type, channel: form.channel, message: form.message, subject: form.subject || undefined }),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['notifications'] }); setModalOpen(false); setForm({ memberId: '', type: 'Reminder', channel: 'WhatsApp', subject: '', message: '' }); },
  });

  const resendMutation = useMutation({
    mutationFn: (id: string) => notificationsApi.resend(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['notifications'] }),
  });

  const items = data?.items || [];
  const members = membersData?.items || [];

  const columns = [
    { header: 'Member', accessor: (n: NotificationDto) => n.memberName || '-' },
    { header: 'Type', accessor: 'type' as keyof NotificationDto },
    { header: 'Channel', accessor: 'channel' as keyof NotificationDto },
    { header: 'Message', accessor: (n: NotificationDto) => <div className="max-w-xs truncate">{n.message}</div> as any },
    { header: 'Status', accessor: (n: NotificationDto) => <Badge>{n.status}</Badge> },
    { header: 'Sent', accessor: (n: NotificationDto) => n.sentAt ? formatDateTime(n.sentAt) : '-' },
    { header: 'Actions', accessor: (n: NotificationDto) => (
      <div onClick={(e) => e.stopPropagation()}>
        {n.status === 'Failed' && <Button size="sm" variant="ghost" onClick={() => resendMutation.mutate(n.id)}>Resend</Button>}
      </div>
    ) as any },
  ];

  const statusOptions = [
    { value: '', label: 'All Statuses' },
    { value: 'Pending', label: 'Pending' },
    { value: 'Sent', label: 'Sent' },
    { value: 'Failed', label: 'Failed' },
  ];

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-900">Notifications</h1>
        <Button onClick={() => setModalOpen(true)}>Send Notification</Button>
      </div>
      <Card>
        <CardHeader>
          <Select options={statusOptions} value={statusFilter} onChange={(e) => { setStatusFilter(e.target.value); setPage(1); }} className="max-w-xs" />
        </CardHeader>
        <CardContent className="p-0">
          <Table columns={columns} data={items} loading={isLoading} keyExtractor={(n) => n.id} />
          {data && (
            <div className="flex items-center justify-between px-4 py-3 border-t border-gray-200">
              <span className="text-sm text-gray-600">Page {data.page} of {data.totalPages}</span>
              <div className="flex gap-2">
                <Button size="sm" variant="secondary" disabled={!data.hasPreviousPage} onClick={() => setPage((p) => p - 1)}>Previous</Button>
                <Button size="sm" variant="secondary" disabled={!data.hasNextPage} onClick={() => setPage((p) => p + 1)}>Next</Button>
              </div>
            </div>
          )}
        </CardContent>
      </Card>

      <Modal open={modalOpen} onClose={() => setModalOpen(false)} title="Send Notification">
        <form onSubmit={(e) => { e.preventDefault(); sendMutation.mutate(); }} className="space-y-4">
          <Select label="Member" id="memberId" value={form.memberId} onChange={(e) => setForm({ ...form, memberId: e.target.value })} options={members.map((m: any) => ({ value: m.id, label: `${m.fullName} (${m.code})` }))} placeholder="Select member" required />
          <div className="grid grid-cols-2 gap-4">
            <Select label="Type" id="type" value={form.type} onChange={(e) => setForm({ ...form, type: e.target.value })} options={[{ value: 'Reminder', label: 'Reminder' }, { value: 'Expiration', label: 'Expiration' }, { value: 'Payment', label: 'Payment' }, { value: 'Promotion', label: 'Promotion' }, { value: 'General', label: 'General' }]} />
            <Select label="Channel" id="channel" value={form.channel} onChange={(e) => setForm({ ...form, channel: e.target.value })} options={[{ value: 'WhatsApp', label: 'WhatsApp' }, { value: 'SMS', label: 'SMS' }, { value: 'Email', label: 'Email' }, { value: 'Push', label: 'Push' }]} />
          </div>
          <Input label="Subject" id="subject" value={form.subject} onChange={(e) => setForm({ ...form, subject: e.target.value })} />
          <div className="space-y-1">
            <label htmlFor="message" className="block text-sm font-medium text-gray-700">Message</label>
            <textarea id="message" rows={4} value={form.message} onChange={(e) => setForm({ ...form, message: e.target.value })} className="block w-full rounded-lg border border-gray-300 px-3 py-2 text-sm shadow-sm focus:border-indigo-500 focus:outline-none focus:ring-1 focus:ring-indigo-500" required />
          </div>
          <div className="flex justify-end gap-3">
            <Button type="button" variant="secondary" onClick={() => setModalOpen(false)}>Cancel</Button>
            <Button type="submit" loading={sendMutation.isPending}>Send</Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
