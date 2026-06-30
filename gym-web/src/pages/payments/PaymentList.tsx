import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { paymentsApi } from '../../api/payments';
import { membersApi } from '../../api/members';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { Select } from '../../components/ui/Select';
import { Modal } from '../../components/ui/Modal';
import { Table } from '../../components/ui/Table';
import { Card, CardContent, CardHeader } from '../../components/ui/Card';
import { formatDate } from '../../lib/utils';
import type { PaymentDto } from '../../types';

export default function PaymentList() {
  const [page, setPage] = useState(1);
  const [dateFrom, setDateFrom] = useState('');
  const [dateTo, setDateTo] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [form, setForm] = useState({ memberId: '', amount: 0, amountPaid: 0, paymentDate: new Date().toISOString().split('T')[0], method: 'Cash', reference: '', notes: '' });
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery({
    queryKey: ['payments', page, dateFrom, dateTo],
    queryFn: () => paymentsApi.getAll({ page, pageSize: 10, dateFrom: dateFrom || undefined, dateTo: dateTo || undefined }).then((r) => r.data.data),
  });

  const { data: membersData } = useQuery({ queryKey: ['members-small2'], queryFn: () => membersApi.getAll({ pageSize: 100 }).then((r) => r.data.data) });

  const createMutation = useMutation({
    mutationFn: () => paymentsApi.create({
      memberId: form.memberId,
      amount: form.amount,
      discountAmount: Math.max(0, form.amount - form.amountPaid),
      paymentMethod: form.method,
      referenceNumber: form.reference || null,
      notes: form.notes || null,
      receiptNumber: `RCP-${Date.now()}`,
    }),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['payments'] }); setModalOpen(false); setForm({ memberId: '', amount: 0, amountPaid: 0, paymentDate: new Date().toISOString().split('T')[0], method: 'Cash', reference: '', notes: '' }); },
  });

  const items = data?.items || [];
  const members = membersData?.items || [];

  const columns = [
    { header: 'Member', accessor: (p: PaymentDto) => <span className="font-medium">{p.memberName}</span> },
    { header: 'Date', accessor: (p: PaymentDto) => formatDate(p.paymentDate) },
    { header: 'Amount', accessor: (p: PaymentDto) => `EGP ${p.amount.toFixed(2)}` },
    { header: 'Paid', accessor: (p: PaymentDto) => `EGP ${p.totalAmount.toFixed(2)}` },
    { header: 'Discount', accessor: (p: PaymentDto) => p.discountAmount > 0 ? `EGP ${p.discountAmount.toFixed(2)}` : '-' },
    { header: 'Method', accessor: (p: PaymentDto) => p.paymentMethod },
    { header: 'Receipt', accessor: (p: PaymentDto) => p.receiptNumber },
    { header: 'Ref', accessor: (p: PaymentDto) => p.referenceNumber || '-' },
  ];

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-900">Payments</h1>
        <Button onClick={() => setModalOpen(true)}>Record Payment</Button>
      </div>
      <Card>
        <CardHeader className="flex flex-wrap items-center gap-3">
          <Input type="date" value={dateFrom} onChange={(e) => { setDateFrom(e.target.value); setPage(1); }} className="w-40" placeholder="From" />
          <Input type="date" value={dateTo} onChange={(e) => { setDateTo(e.target.value); setPage(1); }} className="w-40" placeholder="To" />
          <Button size="sm" variant="ghost" onClick={() => { setDateFrom(''); setDateTo(''); setPage(1); }}>Clear</Button>
        </CardHeader>
        <CardContent className="p-0">
          <Table columns={columns} data={items} loading={isLoading} keyExtractor={(p) => p.id} />
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

      <Modal open={modalOpen} onClose={() => setModalOpen(false)} title="Record Payment">
        <form onSubmit={(e) => { e.preventDefault(); createMutation.mutate(); }} className="space-y-4">
          <Select label="Member" id="memberId" value={form.memberId} onChange={(e) => setForm({ ...form, memberId: e.target.value })} options={members.map((m: any) => ({ value: m.id, label: `${m.fullName} (${m.code})` }))} placeholder="Select member" required />
          <div className="grid grid-cols-2 gap-4">
            <Input label="Amount" id="amount" type="number" step="0.01" value={form.amount} onChange={(e) => setForm({ ...form, amount: Number(e.target.value) })} required />
            <Input label="Amount Paid" id="amountPaid" type="number" step="0.01" value={form.amountPaid} onChange={(e) => setForm({ ...form, amountPaid: Number(e.target.value) })} required />
          </div>
          <Input label="Payment Date" id="paymentDate" type="date" value={form.paymentDate} onChange={(e) => setForm({ ...form, paymentDate: e.target.value })} required />
          <Select label="Method" id="method" value={form.method} onChange={(e) => setForm({ ...form, method: e.target.value })} options={[{ value: 'Cash', label: 'Cash' }, { value: 'Visa', label: 'Visa' }, { value: 'Instapay', label: 'Instapay' }, { value: 'Wallet', label: 'Wallet' }]} />
          <Input label="Reference" id="reference" value={form.reference} onChange={(e) => setForm({ ...form, reference: e.target.value })} />
          <Input label="Notes" id="notes" value={form.notes} onChange={(e) => setForm({ ...form, notes: e.target.value })} />
          <div className="flex justify-end gap-3">
            <Button type="button" variant="secondary" onClick={() => setModalOpen(false)}>Cancel</Button>
            <Button type="submit" loading={createMutation.isPending}>Record</Button>
          </div>
        </form>
      </Modal>
    </div>
  );
}
