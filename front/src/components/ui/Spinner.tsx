import { cn } from '@/lib/utils'

interface SpinnerProps {
  size?: 'sm' | 'md' | 'lg'
  className?: string
  label?: string
}

const sizes = { sm: 'size-4', md: 'size-8', lg: 'size-12' }

export function Spinner({ size = 'md', className, label = 'Carregando...' }: SpinnerProps) {
  return (
    <div className={cn('flex flex-col items-center justify-center gap-2', className)}>
      <svg
        className={cn('animate-spin text-primary', sizes[size])}
        xmlns="http://www.w3.org/2000/svg"
        fill="none"
        viewBox="0 0 24 24"
        aria-hidden="true"
      >
        <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" className="opacity-25" />
        <path
          fill="currentColor"
          className="opacity-75"
          d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"
        />
      </svg>
      {label && <span className="text-sm text-slate-500">{label}</span>}
    </div>
  )
}
