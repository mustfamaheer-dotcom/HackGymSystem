import { cn } from '../../lib/utils';

interface BadgeProps {
  children: string;
  className?: string;
}

export function Badge({ children, className }: BadgeProps) {
  const colorMap: Record<string, string> = {
    Active: 'bg-green-100 text-green-800',
    Inactive: 'bg-gray-100 text-gray-800',
    Expired: 'bg-red-100 text-red-800',
    Frozen: 'bg-blue-100 text-blue-800',
    Cancelled: 'bg-yellow-100 text-yellow-800',
    Pending: 'bg-orange-100 text-orange-800',
    Paid: 'bg-green-100 text-green-800',
    Overdue: 'bg-red-100 text-red-800',
    Sent: 'bg-green-100 text-green-800',
    Failed: 'bg-red-100 text-red-800',
  };

  const colorClass = colorMap[children] || 'bg-gray-100 text-gray-800';

  return (
    <span className={cn('inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium', colorClass, className)}>
      {children}
    </span>
  );
}
