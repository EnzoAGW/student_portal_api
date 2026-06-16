import { cn } from '@/lib/utils'

// Variantes visuais do botão
type Variant = 'primary' | 'secondary' | 'danger' | 'ghost' | 'success'
type Size = 'sm' | 'md' | 'lg'

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: Variant
  size?: Size
  loading?: boolean
}

// Mapeamento de variante → classes Tailwind
const variantClasses: Record<Variant, string> = {
  primary:   'bg-primary text-white hover:bg-primary-dark shadow-sm',
  secondary: 'bg-white text-slate-700 border border-slate-300 hover:bg-slate-50 shadow-sm',
  danger:    'bg-danger text-white hover:bg-red-700 shadow-sm',
  ghost:     'text-slate-600 hover:bg-slate-100',
  success:   'bg-success text-white hover:bg-green-700 shadow-sm',
}

const sizeClasses: Record<Size, string> = {
  sm:  'px-3 py-1.5 text-sm',
  md:  'px-4 py-2 text-sm',
  lg:  'px-6 py-3 text-base',
}

export function Button({
  variant = 'primary',
  size = 'md',
  loading = false,
  disabled,
  className,
  children,
  ...props
}: ButtonProps) {
  return (
    <button
      {...props}
      disabled={disabled || loading}
      className={cn(
        'inline-flex items-center justify-center gap-2 rounded-lg font-medium',
        'transition-colors focus-visible:outline-none focus-visible:ring-2',
        'focus-visible:ring-primary focus-visible:ring-offset-2',
        'disabled:opacity-50 disabled:cursor-not-allowed',
        variantClasses[variant],
        sizeClasses[size],
        className,
      )}
    >
      {/* Spinner dentro do botão quando em carregamento */}
      {loading && (
        <svg className="animate-spin size-4" viewBox="0 0 24 24" fill="none">
          <circle cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" className="opacity-25" />
          <path fill="currentColor" className="opacity-75"
            d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
        </svg>
      )}
      {children}
    </button>
  )
}
