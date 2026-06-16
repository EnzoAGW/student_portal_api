import { cn } from '@/lib/utils'

interface CardProps {
  children: React.ReactNode
  className?: string
}

interface CardHeaderProps {
  title: string
  subtitle?: string
  action?: React.ReactNode  // ex: um botão no canto direito do cabeçalho
}

// Card base — caixa branca com sombra suave
export function Card({ children, className }: CardProps) {
  return (
    <div className={cn('bg-white rounded-xl border border-slate-200 shadow-sm', className)}>
      {children}
    </div>
  )
}

// Cabeçalho do card com título opcional + ação
export function CardHeader({ title, subtitle, action }: CardHeaderProps) {
  return (
    <div className="flex items-center justify-between px-6 py-4 border-b border-slate-100">
      <div>
        <h3 className="font-semibold text-slate-800">{title}</h3>
        {subtitle && <p className="text-sm text-slate-500 mt-0.5">{subtitle}</p>}
      </div>
      {action}
    </div>
  )
}

export function CardBody({ children, className }: CardProps) {
  return (
    <div className={cn('px-6 py-4', className)}>
      {children}
    </div>
  )
}
