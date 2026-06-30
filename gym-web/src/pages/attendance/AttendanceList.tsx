import { useState } from 'react';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { attendanceApi } from '../../api/attendance';
import { membersApi } from '../../api/members';
import { Button } from '../../components/ui/Button';
import { Input } from '../../components/ui/Input';
import { Select } from '../../components/ui/Select';
import { Table } from '../../components/ui/Table';
import { Card, CardContent, CardHeader } from '../../components/ui/Card';
import { Badge } from '../../components/ui/Badge';
import { formatDateTime } from '../../lib/utils';
import type { AttendanceDto } from '../../types';

export default function AttendanceList() {
  const [page, setPage] = useState(1);
  const [dateFrom, setDateFrom] = useState('');
  const [dateTo, setDateTo] = useState('');
  const [showCheckIn, setShowCheckIn] = useState(false);
  const [checkInMemberId, setCheckInMemberId] = useState('');
  const [searchTerm, setSearchTerm] = useState('');
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery({
    queryKey: ['attendance', page, dateFrom, dateTo],
    queryFn: () => attendanceApi.getAll({
      page, pageSize: 15,
      dateFrom: dateFrom || undefined,
      dateTo: dateTo || undefined,
    }).then((r) => r.data.data),
  });

  const { data: membersData } = useQuery({
    queryKey: ['members-search', searchTerm],
    queryFn: () => membersApi.getAll({ pageSize: 50, searchTerm: searchTerm || undefined }).then((r) => r.data.data),
  });

  const checkInMutation = useMutation({
    mutationFn: () => attendanceApi.checkIn({ memberId: checkInMemberId, isManual: true }),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['attendance'] }); setShowCheckIn(false); setCheckInMemberId(''); },
  });

  const checkOutMutation = useMutation({
    mutationFn: (id: string) => attendanceApi.checkOut({ id }),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['attendance'] }),
  });

  const items = data?.items || [];
  const members = membersData?.items || [];

  const columns = [
    { header: 'Member', accessor: (a: AttendanceDto) => a.memberName },
    { header: 'Check In', accessor: (a: AttendanceDto) => a.checkIn ? formatDateTime(a.checkIn) : '-' },
    { header: 'Check Out', accessor: (a: AttendanceDto) => a.checkOut ? formatDateTime(a.checkOut) : '-' },
    { header: 'Method', accessor: (a: AttendanceDto) => <Badge>{a.isManual ? 'Manual' : 'Auto'}</Badge> },
    { header: 'Device', accessor: (a: AttendanceDto) => a.deviceName || '-' },
    { header: 'Actions', accessor: (a: AttendanceDto) => (
      <div onClick={(e) => e.stopPropagation()}>
        {!a.checkOut && <Button size="sm" variant="primary" onClick={() => checkOutMutation.mutate(a.id)}>Check Out</Button>}
      </div>
    ) as any },
  ];

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold text-gray-900">Attendance</h1>
        <Button onClick={() => setShowCheckIn(true)}>Manual Check-in</Button>
      </div>
      <Card>
        <CardHeader className="flex flex-wrap items-center gap-3">
          <Input type="date" value={dateFrom} onChange={(e) => { setDateFrom(e.target.value); setPage(1); }} className="w-40" placeholder="From" />
          <Input type="date" value={dateTo} onChange={(e) => { setDateTo(e.target.value); setPage(1); }} className="w-40" placeholder="To" />
          <Button size="sm" variant="ghost" onClick={() => { setDateFrom(''); setDateTo(''); setPage(1); }}>Clear</Button>
        </CardHeader>
        <CardContent className="p-0">
          <Table columns={columns} data={items} loading={isLoading} keyExtractor={(a) => a.id} />
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

      {showCheckIn && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50" onClick={() => setShowCheckIn(false)}>
          <div className="bg-white rounded-xl p-6 w-full max-w-sm mx-4" onClick={(e) => e.stopPropagation()}>
            <h2 className="text-lg font-semibold text-gray-900 mb-4">Manual Check-in</h2>
            <div className="space-y-4">
              <Input placeholder="Search member..." value={searchTerm} onChange={(e) => setSearchTerm(e.target.value)} />
              <Select id="memberId" value={checkInMemberId} onChange={(e) => setCheckInMemberId(e.target.value)} options={members.map((m: any) => ({ value: m.id, label: `${m.fullName} (${m.code})` }))} placeholder="Select member" />
              <div className="flex justify-end gap-3">
                <Button variant="secondary" onClick={() => setShowCheckIn(false)}>Cancel</Button>
                <Button onClick={() => checkInMutation.mutate()} disabled={!checkInMemberId} loading={checkInMutation.isPending}>Check In</Button>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
