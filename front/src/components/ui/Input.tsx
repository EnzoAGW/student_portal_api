import { forwardRef } from 'react'
import { cn } from '@/lib/utils'

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  label?: string
  error?: string
  hint?: string
}

// forwardRef permite que bibliotecas como React Hook Form acessem o input diretamente
export const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ label, error, hint, className, id, ...props }, ref) => {
    const inputId = id || label?.toLowerCase().replace(/\s/g, '-')

    return (
      <div className="flex flex-col gap-1">
        {label && (
          <label htmlFor={inputId} className="text-sm font-medium text-slate-700">
            {label}
          </label>
        )}
        <input
          ref={ref}
          id={inputId}
          className={cn(
            'w-full rounded-lg border px-3 py-2 text-sm text-slate-900',
            'placeholder:text-slate-400 transition-colors',
            'focus:outline-none focus:ring-2 focus:ring-primary focus:border-transparent',
            error
              ? 'border-danger bg-danger-light/30'
              : 'border-slate-300 bg-white hover:border-slate-400',
            className,
          )}
          {...props}
        />
        {hint && !error && <p className="text-xs text-slate-500">{hint}</p>}
        {error && <p className="text-xs text-danger">{error}</p>}
      </div>
    )
  }
)
Input.displayName = 'Input'
