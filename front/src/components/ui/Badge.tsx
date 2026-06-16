import { cn } from '@/lib/utils'

type BadgeVariant = 'default' | 'success' | 'danger' | 'warning' | 'info' | 'primary'

interface BadgeProps {
  children: React.ReactNode
  variant?: BadgeVariant
  className?: string
}

const variants: Record<BadgeVariant, string> = {
  default:  'bg-slate-100 text-slate-700',
  primary:  'bg-primary-light text-primary-dark',
  success:  'bg-success-light text-success',
  danger:   'bg-danger-light text-danger',
  warning:  'bg-warning-light text-warning',
  info:     'bg-info-light text-info',
}

export function Badge({ children, variant = 'default', className }: BadgeProps) {
  return (
    <span
      className={cn(
        'inline-flex items-center rounded-full px-2.5 py-0.5 text-xs font-medium',
        variants[variant],
        className,
      )}
    >
      {children}
    </span>
  )
}
