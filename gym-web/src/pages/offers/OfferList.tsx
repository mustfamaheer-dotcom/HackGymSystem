import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { offersApi } from '../../api/offers';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { Select } from '../../components/ui/Select';
import { Modal } from '../../components/ui/Modal';
import { Table } from '../../components/ui/Table';
import { Card, CardContent } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';
import { formatDate } from '../../lib/utils';
import type { OfferDto, OfferFormData } from '../../types';

export default function OfferList() {
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<OfferDto | null>(null);
  const [form, setForm] = useState<OfferFormData>({ title: '', description: '', discountType: 'Percentage', discountValue: 0, startDate: new Date().toISOString().split('T')[0], endDate: '' });
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery({
    queryKey: ['offers'],
    queryFn: () => offersApi.getAll({ pageSize: 100 }).then((r) => r.data.data),
  });

  const createMutation = useMutation({
    mutationFn: (d: OfferFormData) => offersApi.create(d),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['offers'] }); closeModal(); },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data: d }: { id: string; data: OfferFormData }) => offersApi.update(id, d),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['offers'] }); closeModal(); },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => offersApi.delete(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['offers'] }),
  });

  const openCreate = () => {
    setEditing(null);
    setForm({ title: '', description: '', discountType: 'Percentage', discountValue: 0, startDate: new Date().toISOString().split('T')[0], endDate: '' });
    setModalOpen(true);
  };

  const openEdit = (offer: OfferDto) => {
    setEditing(offer);
    setForm({ title: offer.title, description: offer.description || '', discountType: offer.discountType, discountValue: offer.discountValue, startDate: offer.startDate.split('T')[0], endDate: offer.endDate.split('T')[0] });
    setModalOpen(true);
  };

  const closeModal = () => { setModalOpen(false); setEditing(null); };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (editing) updateMutation.mutate({ id: editing.id, data: form });
    else createMutation.mutate(form);
  };

  const offers = data?.items || [];

  const columns = [
    { header: 'Title', accessor: 'title' as keyof OfferDto },
    { header: 'Discount', accessor: (o: OfferDto) => o.discountType === 'Percentage' ? `${o.discountValue}%` : `EGP ${o.discountValue.toFixed(2)}` },
    { header: 'Period', accessor: (o: OfferDto) => `${formatDate(o.startDate)} - ${formatDate(o.endDate)}` },
    { header: 'Status', accessor: (o: OfferDto) => <Badge>{o.isActive ? 'Active' : 'Inactive'}</Badge> },
    { header: 'Actions', accessor: (o: OfferDto) => (
      <div className="flex gap-2" onClick={(e) => e.stopPropagation()}>
        <Button size="sm" variant="ghost" onClick={() => openEdit(o)}>Edit</Button>
        <Button size="sm" variant="danger" onClick={() => { if (confirm('Delete this offer?')) deleteMutation.mutate(o.id); }}>Delete</Button>
      </div>
    ) as any },
  ];

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-900">Offers & Discounts</h1>
        <Button onClick={openCreate}>Add Offer</Button>
      </div>
      <Card>
        <CardContent className="p-0">
          <Table columns={columns} data={offers} loading={isLoading} keyExtractor={(o) => o.id} />
        </CardContent>
      </Card>

      <Modal open={modalOpen} onClose={closeModal} title={editing ? 'Edit Offer' : 'Add Offer'}>
        <form onSubmit={handleSubmit} className="space-y-4">
          <Input label="Title" id="title" value={form.title} onChange={(e) => setForm({ ...form, title: e.target.value })} required />
          <Input label="Description" id="description" value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />
          <div className="grid grid-cols-2 gap-4">
            <Select label="Discount Type" id="discountType" value={form.discountType} onChange={(e) => setForm({ ...form, discountType: e.target.value })} options={[{ value: 'Percentage', label: 'Percentage' }, { value: 'Fixed', label: 'Fixed Amount' }]} />
            <Input label="Discount Value" id="discountValue" type="number" step="0.01" value={form.discountValue} onChange={(e) => setForm({ ...form, discountValue: Number(e.target.value) })} required />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <Input label="Start Date" id="startDate" type="date" value={form.startDate} onChange={(e) => setForm({ ...form, startDate: e.target.value })} required />
            <Input label="End Date" id="endDate" type="date" value={form.endDate} onChange={(e) => setForm({ ...form, endDate: e.target.value })} />
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
