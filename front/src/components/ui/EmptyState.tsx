interface EmptyStateProps {
  icon?: string       // emoji ou texto
  title: string
  description?: string
}

// Componente exibido quando uma lista está vazia
// Ex: "Nenhuma matéria encontrada"
export function EmptyState({ icon = '📭', title, description }: EmptyStateProps) {
  return (
    <div className="flex flex-col items-center justify-center py-12 text-center">
      <span className="text-4xl mb-3">{icon}</span>
      <p className="font-medium text-slate-700">{title}</p>
      {description && <p className="text-sm text-slate-500 mt-1">{description}</p>}
    </div>
  )
}
