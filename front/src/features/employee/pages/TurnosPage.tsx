import { useWorkShifts } from '../hooks'
import { Card, CardBody } from '@/components/ui/Card'
import { Badge } from '@/components/ui/Badge'
import { Spinner } from '@/components/ui/Spinner'
import { EmptyState } from '@/components/ui/EmptyState'
import { formatTimeSpan } from '@/lib/utils'

export default function TurnosPage() {
  const { data: shifts, isLoading } = useWorkShifts()

  return (
    <div className="p-6">
      <h2 className="text-xl font-bold text-slate-800 mb-6">Turnos Cadastrados</h2>

      {isLoading ? (
        <Spinner className="py-16" label="Carregando turnos..." />
      ) : !shifts?.length ? (
        <EmptyState icon="🔄" title="Sem turnos" description="Nenhum turno cadastrado ainda." />
      ) : (
        <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
          {shifts.map((shift) => (
            <Card key={shift.id}>
              <CardBody>
                <div className="flex items-center justify-between mb-3">
                  <h3 className="font-semibold text-slate-800">{shift.name}</h3>
                  <Badge variant="info">{shift.expectedHours}h</Badge>
                </div>

                {/* Horário do turno */}
                <div className="flex items-center gap-2 text-sm text-slate-600">
                  <span>⏰</span>
                  <span className="font-mono">
                    {formatTimeSpan(shift.startTime)} – {formatTimeSpan(shift.endTime)}
                  </span>
                </div>

                <div className="mt-3 pt-3 border-t border-slate-100">
                  <p className="text-xs text-slate-400">
                    Carga horária esperada: <strong>{shift.expectedHours}h/dia</strong>
                  </p>
                </div>
              </CardBody>
            </Card>
          ))}
        </div>
      )}
    </div>
  )
}
