import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import toast from 'react-hot-toast';
import { usersApi } from '../../api/users';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { Select } from '../../components/ui/Select';
import { Modal } from '../../components/ui/Modal';
import { Table } from '../../components/ui/Table';
import { Card, CardContent } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';
import { formatDate } from '../../lib/utils';
import type { UserListItemDto } from '../../types';

export default function UserList() {
  const [modalOpen, setModalOpen] = useState(false);
  const [editing, setEditing] = useState<UserListItemDto | null>(null);
  const [search, setSearch] = useState('');
  const [form, setForm] = useState({ username: '', password: '', confirmPassword: '', fullName: '', email: '', phone: '', roleId: '' });
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery({
    queryKey: ['users', search],
    queryFn: () => usersApi.getAll({ pageSize: 100, searchTerm: search || undefined }).then((r) => r.data.data),
  });

  const { data: rolesData } = useQuery({
    queryKey: ['roles'],
    queryFn: () => usersApi.getRoles().then((r) => r.data.data),
  });

  const formatError = (error: any) => {
    const data = error?.response?.data;
    if (data?.errors) {
      const lines: string[] = [];
      for (const [, msgs] of Object.entries(data.errors)) {
        if (Array.isArray(msgs)) lines.push(...msgs);
        else lines.push(String(msgs));
      }
      return lines.join('\n');
    }
    return data?.message || error?.message || 'An unexpected error occurred';
  };

  const createMutation = useMutation({
    mutationFn: (d: typeof form) => usersApi.create({
      username: d.username,
      password: d.password,
      fullName: d.fullName,
      email: d.email,
      phone: d.phone || undefined,
      roleId: d.roleId,
    }),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['users'] }); closeModal(); },
    onError: (error: any) => toast.error(formatError(error), { duration: 6000 }),
  });

  const updateMutation = useMutation({
    mutationFn: ({ id, d, isActive }: { id: string; d: typeof form; isActive: boolean }) => usersApi.update(id, {
      fullName: d.fullName,
      email: d.email,
      phone: d.phone || undefined,
      roleId: d.roleId,
      isActive,
    }),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['users'] }); closeModal(); },
    onError: (error: any) => toast.error(formatError(error), { duration: 6000 }),
  });

  const deactivateMutation = useMutation({
    mutationFn: (user: UserListItemDto) => usersApi.update(user.id, {
      fullName: user.fullName,
      email: user.email,
      phone: user.phone || undefined,
      roleId: user.roleId,
      isActive: false,
    }),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
  });

  const activateMutation = useMutation({
    mutationFn: (user: UserListItemDto) => usersApi.update(user.id, {
      fullName: user.fullName,
      email: user.email,
      phone: user.phone || undefined,
      roleId: user.roleId,
      isActive: true,
    }),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
  });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => usersApi.delete(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
  });

  const openCreate = () => {
    setEditing(null);
    setForm({ username: '', password: '', confirmPassword: '', fullName: '', email: '', phone: '', roleId: rolesData?.[0]?.id || '' });
    setModalOpen(true);
  };

  const openEdit = (user: UserListItemDto) => {
    setEditing(user);
    setForm({ username: user.username, password: '', confirmPassword: '', fullName: user.fullName, email: user.email, phone: user.phone || '', roleId: user.roleId });
    setModalOpen(true);
  };

  const closeModal = () => { setModalOpen(false); setEditing(null); };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!editing && form.password !== form.confirmPassword) {
      alert('Passwords do not match');
      return;
    }
    if (editing) updateMutation.mutate({ id: editing.id, d: form, isActive: editing.isActive });
    else createMutation.mutate(form);
  };

  const items = data?.items || [];
  const roles = rolesData || [];

  const columns = [
    { header: 'Username', accessor: 'username' as keyof UserListItemDto },
    { header: 'Full Name', accessor: 'fullName' as keyof UserListItemDto },
    { header: 'Email', accessor: 'email' as keyof UserListItemDto },
    { header: 'Role', accessor: 'roleName' as keyof UserListItemDto },
    { header: 'Status', accessor: (u: UserListItemDto) => <Badge>{u.isActive ? 'Active' : 'Inactive'}</Badge> },
    { header: 'Created', accessor: (u: UserListItemDto) => formatDate(u.createdAt) },
    { header: 'Last Login', accessor: (u: UserListItemDto) => u.lastLoginAt ? formatDate(u.lastLoginAt) : '-' },
    { header: 'Actions', accessor: (u: UserListItemDto) => (
      <div className="flex gap-1.5" onClick={(e) => e.stopPropagation()}>
        <Button size="xs" variant="ghost" onClick={() => openEdit(u)}>Edit</Button>
        {u.isActive ? (
          <Button size="xs" variant="danger" onClick={() => { if (confirm('Deactivate this user?')) deactivateMutation.mutate(u); }}>Deactivate</Button>
        ) : (
          <Button size="xs" variant="secondary" onClick={() => activateMutation.mutate(u)}>Activate</Button>
        )}
        <Button size="xs" variant="danger" onClick={() => { if (confirm('Permanently delete this user? This cannot be undone.')) deleteMutation.mutate(u.id); }}>Delete</Button>
      </div>
    ) as any },
  ];

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">User Management</h1>
          <p className="mt-1 text-sm text-gray-500">Manage system users and their roles</p>
        </div>
        <Button onClick={openCreate}>Add User</Button>
      </div>
      <Card>
        <CardContent className="p-0">
          <div className="p-4 border-b border-gray-200">
            <Input placeholder="Search users..." value={search} onChange={(e) => setSearch(e.target.value)} />
          </div>
          <Table columns={columns} data={items} loading={isLoading} keyExtractor={(u) => u.id} />
        </CardContent>
      </Card>

      <Modal open={modalOpen} onClose={closeModal} title={editing ? 'Edit User' : 'Create New User'} size="lg">
        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="bg-gradient-to-r from-indigo-50 to-blue-50 rounded-lg p-5 -mx-1">
            <div className="flex items-center gap-3 mb-4">
              <div className="w-10 h-10 rounded-full bg-indigo-100 flex items-center justify-center">
                <svg className="w-5 h-5 text-indigo-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" />
                </svg>
              </div>
              <div>
                <h3 className="text-sm font-semibold text-gray-700">Account Information</h3>
                <p className="text-xs text-gray-400">Basic user details and login credentials</p>
              </div>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Input label="Full Name" id="fullName" value={form.fullName} onChange={(e) => setForm({ ...form, fullName: e.target.value })} required placeholder="John Doe" />
              <Input label="Username" id="username" value={form.username} onChange={(e) => setForm({ ...form, username: e.target.value })} required disabled={!!editing} placeholder="johndoe" />
            </div>
          </div>

          <div className="bg-gradient-to-r from-amber-50 to-orange-50 rounded-lg p-5 -mx-1">
            <div className="flex items-center gap-3 mb-4">
              <div className="w-10 h-10 rounded-full bg-amber-100 flex items-center justify-center">
                <svg className="w-5 h-5 text-amber-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
                </svg>
              </div>
              <div>
                <h3 className="text-sm font-semibold text-gray-700">Security</h3>
                <p className="text-xs text-gray-400">{editing ? 'Leave blank to keep current password' : 'Set a strong password'}</p>
              </div>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Input label="Password" id="password" type="password" value={form.password} onChange={(e) => setForm({ ...form, password: e.target.value })} required={!editing} placeholder={editing ? 'Leave blank to keep current' : '••••••••'} />
              <Input label="Confirm Password" id="confirmPassword" type="password" value={form.confirmPassword} onChange={(e) => setForm({ ...form, confirmPassword: e.target.value })} required={!editing} placeholder={editing ? 'Leave blank to keep current' : '••••••••'} />
            </div>
          </div>

          <div className="bg-gradient-to-r from-emerald-50 to-teal-50 rounded-lg p-5 -mx-1">
            <div className="flex items-center gap-3 mb-4">
              <div className="w-10 h-10 rounded-full bg-emerald-100 flex items-center justify-center">
                <svg className="w-5 h-5 text-emerald-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 8l7.89 5.26a2 2 0 002.22 0L21 8M5 19h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" />
                </svg>
              </div>
              <div>
                <h3 className="text-sm font-semibold text-gray-700">Contact & Role</h3>
                <p className="text-xs text-gray-400">Contact details and access level</p>
              </div>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <Input label="Phone" id="phone" value={form.phone} onChange={(e) => setForm({ ...form, phone: e.target.value })} placeholder="+1 (555) 000-0000" />
              <Input label="Email" id="email" type="email" value={form.email} onChange={(e) => setForm({ ...form, email: e.target.value })} placeholder="user@example.com" />
            </div>
            <div className="mt-4">
              <Select label="Role" id="roleId" value={form.roleId} onChange={(e) => setForm({ ...form, roleId: e.target.value })} options={roles.map((r) => ({ value: r.id, label: r.name }))} placeholder="Select a role" required />
            </div>
          </div>

          <div className="flex items-center justify-end gap-3 pt-2 border-t border-gray-100">
            <Button type="button" variant="secondary" onClick={closeModal}>Cancel</Button>
            <Button type="submit" loading={createMutation.isPending || updateMutation.isPending}>
              {editing ? 'Save Changes' : 'Create User'}
            </Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
