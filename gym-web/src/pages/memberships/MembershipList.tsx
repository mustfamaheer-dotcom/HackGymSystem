import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { membershipsApi } from '../../api/memberships';
import { membersApi } from '../../api/members';
import { plansApi } from '../../api/plans';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { Select } from '../../components/ui/Select';
import { Modal } from '../../components/ui/Modal';
import { Table } from '../../components/ui/Table';
import { Card, CardContent, CardHeader } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';
import { formatDate } from '../../lib/utils';
import type { MembershipDto } from '../../types';

export default function MembershipList() {
  const [page, setPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [freezeModal, setFreezeModal] = useState<MembershipDto | null>(null);
  const [freezeDays, setFreezeDays] = useState(1);
  const [form, setForm] = useState({ memberId: '', planId: '', startDate: '', notes: '' });
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery({
    queryKey: ['memberships', page, statusFilter],
    queryFn: () => membershipsApi.getAll({ page, pageSize: 10, status: statusFilter || undefined }).then((r) => r.data.data),
  });

  const { data: membersData } = useQuery({ queryKey: ['members-small'], queryFn: () => membersApi.getAll({ pageSize: 100 }).then((r) => r.data.data) });
  const { data: plansData } = useQuery({ queryKey: ['plans-active'], queryFn: () => plansApi.getActive().then((r) => r.data.data) });

  const createMutation = useMutation({
    mutationFn: () => membershipsApi.create({ memberId: form.memberId, planId: form.planId, startDate: form.startDate, notes: form.notes || undefined }),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['memberships'] }); setModalOpen(false); setForm({ memberId: '', planId: '', startDate: '', notes: '' }); },
  });

  const cancelMutation = useMutation({
    mutationFn: (id: string) => membershipsApi.cancel(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['memberships'] }),
  });

  const freezeMutation = useMutation({
    mutationFn: ({ id, days }: { id: string; days: number }) => membershipsApi.freeze(id, { days }),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['memberships'] }); setFreezeModal(null); },
  });

  const unfreezeMutation = useMutation({
    mutationFn: (id: string) => membershipsApi.unfreeze(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['memberships'] }),
  });

  const items = data?.items || [];
  const members = membersData?.items || [];
  const plans = plansData || [];

  const statusOptions = [
    { value: '', label: 'All Statuses' },
    { value: 'Active', label: 'Active' },
    { value: 'Frozen', label: 'Frozen' },
    { value: 'Expired', label: 'Expired' },
    { value: 'Cancelled', label: 'Cancelled' },
  ];

  const columns = [
    { header: 'Member', accessor: (m: MembershipDto) => <span className="font-medium">{m.memberName}</span> },
    { header: 'Plan', accessor: 'planName' as keyof MembershipDto },
    { header: 'Start', accessor: (m: MembershipDto) => formatDate(m.startDate) },
    { header: 'End', accessor: (m: MembershipDto) => formatDate(m.endDate) },
    { header: 'Status', accessor: (m: MembershipDto) => <Badge>{m.status}</Badge> },
    { header: 'Actions', accessor: (m: MembershipDto) => (
      <div className="flex gap-1" onClick={(e) => e.stopPropagation()}>
        {m.status === 'Active' && <> <Button size="sm" variant="ghost" onClick={() => setFreezeModal(m)}>Freeze</Button> <Button size="sm" variant="danger" onClick={() => cancelMutation.mutate(m.id)}>Cancel</Button> </>}
        {m.status === 'Frozen' && <Button size="sm" variant="ghost" onClick={() => unfreezeMutation.mutate(m.id)}>Unfreeze</Button>}
      </div>
    ) as any },
  ];

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-900">Memberships</h1>
        <Button onClick={() => setModalOpen(true)}>New Membership</Button>
      </div>
      <Card>
        <CardHeader>
          <Select options={statusOptions} value={statusFilter} onChange={(e) => { setStatusFilter(e.target.value); setPage(1); }} className="max-w-xs" />
        </CardHeader>
        <CardContent className="p-0">
          <Table columns={columns} data={items} loading={isLoading} keyExtractor={(m) => m.id} />
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

      <Modal open={modalOpen} onClose={() => setModalOpen(false)} title="New Membership">
        <form onSubmit={(e) => { e.preventDefault(); createMutation.mutate(); }} className="space-y-4">
          <Select label="Member" id="memberId" value={form.memberId} onChange={(e) => setForm({ ...form, memberId: e.target.value })} options={members.map((m: any) => ({ value: m.id, label: `${m.fullName} (${m.code})` }))} placeholder="Select member" required />
          <Select label="Plan" id="planId" value={form.planId} onChange={(e) => setForm({ ...form, planId: e.target.value })} options={plans.map((p: any) => ({ value: p.id, label: `${p.name} - ${p.durationDays} days (EGP ${p.price})` }))} placeholder="Select plan" required />
          <Input label="Start Date" id="startDate" type="date" value={form.startDate} onChange={(e) => setForm({ ...form, startDate: e.target.value })} required />
          <Input label="Notes" id="notes" value={form.notes} onChange={(e) => setForm({ ...form, notes: e.target.value })} />
          <div className="flex justify-end gap-3">
            <Button type="button" variant="secondary" onClick={() => setModalOpen(false)}>Cancel</Button>
            <Button type="submit" loading={createMutation.isPending}>Create</Button>
          </div>
        </form>
      </Modal>

      <Modal open={!!freezeModal} onClose={() => setFreezeModal(null)} title="Freeze Membership">
        <form onSubmit={(e) => { e.preventDefault(); if (freezeModal) freezeMutation.mutate({ id: freezeModal.id, days: freezeDays }); }}>
          <Input label="Freeze Duration (days)" id="freezeDays" type="number" value={freezeDays} onChange={(e) => setFreezeDays(Number(e.target.value))} required />
          <div className="flex justify-end gap-3 mt-4">
            <Button type="button" variant="secondary" onClick={() => setFreezeModal(null)}>Cancel</Button>
            <Button type="submit" loading={freezeMutation.isPending}>Freeze</Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
