import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { membersApi } from '../../api/members';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { Select } from '../../components/ui/Select';
import { Modal } from '../../components/ui/Modal';
import { Table } from '../../components/ui/Table';
import { Card, CardContent, CardHeader } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';
import { formatDate } from '../../lib/utils';
import type { MemberDto, MemberFormData } from '../../types';

export default function MemberList() {
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingMember, setEditingMember] = useState<MemberDto | null>(null);
  const [form, setForm] = useState<MemberFormData>({
    fullName: '', phone: '', email: '', gender: '', dateOfBirth: '', address: '', notes: '',
  });
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery({
    queryKey: ['members', page, search],
    queryFn: () => membersApi.getAll({ page, pageSize: 10, searchTerm: search || undefined }).then((r) => r.data.data),
  });

  const createMutation = useMutation({
    mutationFn: (d: MemberFormData) => membersApi.create(d),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['members'] }); closeModal(); },
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, data: d }: { id: string; data: MemberFormData }) => membersApi.update(id, d),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['members'] }); closeModal(); },
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => membersApi.delete(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['members'] }),
  });

  const openCreate = () => {
    setEditingMember(null);
    setForm({ fullName: '', phone: '', email: '', gender: '', dateOfBirth: '', address: '', notes: '' });
    setModalOpen(true);
  };

  const openEdit = (member: MemberDto) => {
    setEditingMember(member);
    setForm({
      fullName: member.fullName, phone: member.phone, email: member.email || '',
      gender: member.gender || '', dateOfBirth: member.dateOfBirth || '',
      address: member.address || '', notes: member.notes || '',
    });
    setModalOpen(true);
  };

  const closeModal = () => { setModalOpen(false); setEditingMember(null); };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (editingMember) updateMutation.mutate({ id: editingMember.id, data: form });
    else createMutation.mutate(form);
  };

  const items = data?.items || [];

  const columns = [
    { header: 'Code', accessor: (m: MemberDto) => <span className="font-mono text-sm">{m.code}</span> },
    { header: 'Name', accessor: 'fullName' as keyof MemberDto },
    { header: 'Phone', accessor: 'phone' as keyof MemberDto },
    { header: 'Gender', accessor: (m: MemberDto) => m.gender || '-' },
    { header: 'Status', accessor: (m: MemberDto) => <Badge>{m.status || (m.isActive ? 'Active' : 'Inactive')}</Badge> },
    { header: 'Joined', accessor: (m: MemberDto) => formatDate(m.joinDate) },
    { header: 'Actions', accessor: (m: MemberDto) => (
      <div className="flex gap-2" onClick={(e) => e.stopPropagation()}>
        <Button size="sm" variant="ghost" onClick={() => openEdit(m)}>Edit</Button>
        <Button size="sm" variant="danger" onClick={() => { if (confirm('Delete member?')) deleteMutation.mutate(m.id); }}>Delete</Button>
      </div>
    ) as any },
  ];

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-900">Members</h1>
        <Button onClick={openCreate}>Add Member</Button>
      </div>
      <Card>
        <CardHeader className="flex items-center gap-4">
          <Input placeholder="Search members..." value={search} onChange={(e) => { setSearch(e.target.value); setPage(1); }} className="max-w-xs" />
        </CardHeader>
        <CardContent className="p-0">
          <Table columns={columns} data={items} loading={isLoading} keyExtractor={(m) => m.id} />
          {data && (
            <div className="flex items-center justify-between px-4 py-3 border-t border-gray-200">
              <span className="text-sm text-gray-600">Page {data.page} of {data.totalPages} ({data.totalCount} total)</span>
              <div className="flex gap-2">
                <Button size="sm" variant="secondary" disabled={!data.hasPreviousPage} onClick={() => setPage((p) => p - 1)}>Previous</Button>
                <Button size="sm" variant="secondary" disabled={!data.hasNextPage} onClick={() => setPage((p) => p + 1)}>Next</Button>
              </div>
            </div>
          )}
        </CardContent>
      </Card>

      <Modal open={modalOpen} onClose={closeModal} title={editingMember ? 'Edit Member' : 'Add Member'} size="lg">
        <form onSubmit={handleSubmit} className="grid grid-cols-2 gap-4">
          <Input label="Full Name" id="fullName" value={form.fullName} onChange={(e) => setForm({ ...form, fullName: e.target.value })} required />
          <Input label="Phone" id="phone" value={form.phone} onChange={(e) => setForm({ ...form, phone: e.target.value })} required />
          <Input label="Email" id="email" type="email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} />
          <Input label="Date of Birth" id="dateOfBirth" type="date" value={form.dateOfBirth} onChange={(e) => setForm({ ...form, dateOfBirth: e.target.value })} />
          <Select label="Gender" id="gender" value={form.gender || ''} onChange={(e) => setForm({ ...form, gender: e.target.value })} options={[{ value: 'Male', label: 'Male' }, { value: 'Female', label: 'Female' }]} placeholder="Select gender" />
          <Input label="Address" id="address" value={form.address} onChange={(e) => setForm({ ...form, address: e.target.value })} />
          <div className="col-span-2"><Input label="Notes" id="notes" value={form.notes} onChange={(e) => setForm({ ...form, notes: e.target.value })} /></div>
          <div className="col-span-2 flex justify-end gap-3 mt-2">
            <Button type="button" variant="secondary" onClick={closeModal}>Cancel</Button>
            <Button type="submit" loading={createMutation.isPending || updateMutation.isPending}>{editingMember ? 'Update' : 'Create'}</Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
