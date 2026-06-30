import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { plansApi } from '../../api/plans';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { Modal } from '../../components/ui/Modal';
import { Table } from '../../components/ui/Table';
import { Card, CardContent } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';
import type { MembershipPlanDto, PlanFormData } from '../../types';

export default function PlanList() {
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<MembershipPlanDto | null>(null);
  const [form, setForm] = useState<PlanFormData>({ name: '', description: '', durationDays: 30, price: 0, maxVisits: undefined, freezeDays: 0 });
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery({
    queryKey: ['plans'],
    queryFn: () => plansApi.getAll({ pageSize: 100 }).then((r) => r.data.data),
  });

  const createMutation = useMutation({
    mutationFn: (d: PlanFormData) => plansApi.create(d),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['plans'] }); closeModal(); },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data: d }: { id: string; data: PlanFormData }) => plansApi.update(id, { id, ...d }),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['plans'] }); closeModal(); },
  });

  const toggleMutation = useMutation({
    mutationFn: ({ id, isActive }: { id: string; isActive: boolean }) => plansApi.toggleActive(id, isActive),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['plans'] }),
  });

  const openCreate = () => {
    setEditing(null);
    setForm({ name: '', description: '', durationDays: 30, price: 0, maxVisits: undefined, freezeDays: 0 });
    setModalOpen(true);
  };

  const openEdit = (plan: MembershipPlanDto) => {
    setEditing(plan);
    setForm({ name: plan.name, description: plan.description || '', durationDays: plan.durationDays, price: plan.price, maxVisits: plan.maxVisits, freezeDays: plan.freezeDays || 0 });
    setModalOpen(true);
  };

  const closeModal = () => { setModalOpen(false); setEditing(null); };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (editing) updateMutation.mutate({ id: editing.id, data: form });
    else createMutation.mutate(form);
  };

  const items = data?.items || [];

  const columns = [
    { header: 'Name', accessor: 'name' as keyof MembershipPlanDto },
    { header: 'Duration', accessor: (p: MembershipPlanDto) => `${p.durationDays} days` },
    { header: 'Price', accessor: (p: MembershipPlanDto) => `EGP ${p.price.toFixed(2)}` },
    { header: 'Max Visits', accessor: (p: MembershipPlanDto) => p.maxVisits?.toString() || 'Unlimited' },
    { header: 'Freeze Days', accessor: (p: MembershipPlanDto) => p.freezeDays?.toString() || '0' },
    { header: 'Status', accessor: (p: MembershipPlanDto) => <Badge>{p.isActive ? 'Active' : 'Inactive'}</Badge> },
    { header: 'Actions', accessor: (p: MembershipPlanDto) => (
      <div className="flex gap-2" onClick={(e) => e.stopPropagation()}>
        <Button size="sm" variant="ghost" onClick={() => openEdit(p)}>Edit</Button>
        <Button size="sm" variant="ghost" onClick={() => toggleMutation.mutate({ id: p.id, isActive: !p.isActive })}>{p.isActive ? 'Deactivate' : 'Activate'}</Button>
      </div>
    ) as any },
  ];

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-900">Membership Plans</h1>
        <Button onClick={openCreate}>Add Plan</Button>
      </div>
      <Card>
        <CardContent className="p-0">
          <Table columns={columns} data={items} loading={isLoading} keyExtractor={(p) => p.id} />
        </CardContent>
      </Card>

      <Modal open={modalOpen} onClose={closeModal} title={editing ? 'Edit Plan' : 'Add Plan'}>
        <form onSubmit={handleSubmit} className="space-y-4">
          <Input label="Plan Name" id="name" value={form.name} onChange={(e) => setForm({ ...form, name: e.target.value })} required />
          <Input label="Description" id="description" value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />
          <div className="grid grid-cols-2 gap-4">
            <Input label="Duration (days)" id="durationDays" type="number" value={form.durationDays} onChange={(e) => setForm({ ...form, durationDays: Number(e.target.value) })} required />
            <Input label="Price" id="price" type="number" step="0.01" value={form.price} onChange={(e) => setForm({ ...form, price: Number(e.target.value) })} required />
            <Input label="Max Visits" id="maxVisits" type="number" value={form.maxVisits ?? ''} onChange={(e) => setForm({ ...form, maxVisits: e.target.value ? Number(e.target.value) : undefined })} />
            <Input label="Freeze Days" id="freezeDays" type="number" value={form.freezeDays} onChange={(e) => setForm({ ...form, freezeDays: Number(e.target.value) })} />
          </div>
          <div className="flex justify-end gap-3">
            <Button type="button" variant="secondary" onClick={closeModal}>Cancel</Button>
            <Button type="submit" loading={createMutation.isPending || updateMutation.isPending}>{editing ? 'Update' : 'Create'}</Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
