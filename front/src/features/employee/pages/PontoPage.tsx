import { useEmployeePortalStore } from '@/store/portalStore'
import { useTodayRecord, useClockAction } from '../hooks'
import { EmployeeLookup } from '../components/EmployeeLookup'
import { Card, CardHeader, CardBody } from '@/components/ui/Card'
import { Button } from '@/components/ui/Button'
import { Badge } from '@/components/ui/Badge'
import { Spinner } from '@/components/ui/Spinner'
import { EmptyState } from '@/components/ui/EmptyState'
import { formatTime, formatMinutes } from '@/lib/utils'
import type { TimeRecord } from '@/types/api.types'

// Linha da tabela de ponto: label + horário
function PontoRow({ label, time, filled }: { label: string; time?: string; filled: boolean }) {
  return (
    <div className="flex items-center justify-between py-3 border-b border-slate-100 last:border-0">
      <span className="text-sm text-slate-600">{label}</span>
      {filled ? (
        <span className="font-mono font-semibold text-slate-800">{formatTime(time)}</span>
      ) : (
        <span className="text-slate-400 text-sm">—</span>
      )}
    </div>
  )
}

function PontoCard({ record, employeeId }: { record: TimeRecord | undefined; employeeId: number }) {
  const clock = useClockAction(employeeId)

  // Descobre qual é o próximo passo com base no que já foi registrado
  const nextAction = !record?.clockIn    ? 'clockin'
    : !record?.breakStart               ? 'breakstart'
    : !record?.breakEnd                 ? 'breakend'
    : !record?.clockOut                 ? 'clockout'
    : null

  const actionLabels = {
    clockin:    'Registrar Entrada',
    breakstart: 'Saída p/ Almoço',
    breakend:   'Retorno do Almoço',
    clockout:   'Registrar Saída',
  }

  const actionVariants = {
    clockin:    'primary',
    breakstart: 'secondary',
    breakend:   'secondary',
    clockout:   'danger',
  } as const

  return (
    <Card className="max-w-md">
      <CardHeader
        title="Ponto de Hoje"
        action={
          record?.clockOut
            ? <Badge variant="success">Jornada concluída</Badge>
            : record?.clockIn
            ? <Badge variant="warning">Em andamento</Badge>
            : <Badge variant="default">Não iniciado</Badge>
        }
      />
      <CardBody>
        {/* Registros do dia */}
        <PontoRow label="Entrada"          time={record?.clockIn}    filled={!!record?.clockIn} />
        <PontoRow label="Saída p/ almoço"  time={record?.breakStart} filled={!!record?.breakStart} />
        <PontoRow label="Retorno almoço"   time={record?.breakEnd}   filled={!!record?.breakEnd} />
        <PontoRow label="Saída"            time={record?.clockOut}   filled={!!record?.clockOut} />

        {/* Total de horas trabalhadas */}
        {(record?.workedMinutes ?? 0) > 0 && (
          <div className="mt-4 p-3 bg-slate-50 rounded-lg flex items-center justify-between">
            <span className="text-sm text-slate-600">Total trabalhado</span>
            <span className="font-bold text-primary text-lg">
              {formatMinutes(record!.workedMinutes)}
            </span>
          </div>
        )}

        {/* Botão de ação — muda conforme o estado atual */}
        {nextAction && (
          <Button
            variant={actionVariants[nextAction]}
            className="w-full mt-4"
            loading={clock.isPending}
            onClick={() => clock.mutate(nextAction)}
          >
            {actionLabels[nextAction]}
          </Button>
        )}
      </CardBody>
    </Card>
  )
}

export default function PontoPage() {
  const { selectedEmployeeId } = useEmployeePortalStore()
  const { data: record, isLoading } = useTodayRecord(selectedEmployeeId)

  return (
    <div>
      <EmployeeLookup />

      <div className="p-6">
        <h2 className="text-xl font-bold text-slate-800 mb-6">Meu Ponto</h2>

        {!selectedEmployeeId ? (
          <EmptyState icon="⏰" title="Selecione um funcionário" description="Digite o ID do funcionário acima para ver o ponto." />
        ) : isLoading ? (
          <Spinner className="py-16" label="Carregando ponto..." />
        ) : (
          <PontoCard record={record} employeeId={selectedEmployeeId} />
        )}
      </div>
    </div>
  )
}
