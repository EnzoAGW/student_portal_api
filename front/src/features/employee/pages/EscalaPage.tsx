import { useEmployeePortalStore } from '@/store/portalStore'
import { useSchedule } from '../hooks'
import { EmployeeLookup } from '../components/EmployeeLookup'
import { Card, CardHeader, CardBody } from '@/components/ui/Card'
import { Badge } from '@/components/ui/Badge'
import { Spinner } from '@/components/ui/Spinner'
import { EmptyState } from '@/components/ui/EmptyState'
import { formatDate, formatTimeSpan } from '@/lib/utils'

export default function EscalaPage() {
  const { selectedEmployeeId } = useEmployeePortalStore()
  const { data: schedules, isLoading } = useSchedule(selectedEmployeeId)

  return (
    <div>
      <EmployeeLookup />

      <div className="p-6">
        <h2 className="text-xl font-bold text-slate-800 mb-6">Escala de Trabalho</h2>

        {!selectedEmployeeId ? (
          <EmptyState icon="📆" title="Selecione um funcionário" description="Digite o ID do funcionário acima para ver a escala." />
        ) : isLoading ? (
          <Spinner className="py-16" label="Carregando escala..." />
        ) : !schedules?.length ? (
          <EmptyState icon="📆" title="Sem escala" description="Este funcionário não tem escala cadastrada." />
        ) : (
          <Card>
            <CardHeader title="Próximos dias escalados" subtitle={`${schedules.length} registro(s)`} />
            <CardBody className="p-0">
              <table className="w-full text-sm">
                <thead className="bg-slate-50">
                  <tr>
                    <th className="text-left px-6 py-3 text-slate-600 font-medium">Data</th>
                    <th className="text-left px-6 py-3 text-slate-600 font-medium">Turno</th>
                    <th className="text-left px-6 py-3 text-slate-600 font-medium">Horário</th>
                    <th className="text-left px-6 py-3 text-slate-600 font-medium">Observação</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-slate-100">
                  {schedules.map((s) => (
                    <tr key={s.id} className="hover:bg-slate-50 transition-colors">
                      <td className="px-6 py-3 font-medium text-slate-800">{formatDate(s.date)}</td>
                      <td className="px-6 py-3">
                        <Badge variant="primary">{s.shift?.name ?? `Turno #${s.shiftId}`}</Badge>
                      </td>
                      <td className="px-6 py-3 text-slate-600 font-mono text-xs">
                        {s.shift
                          ? `${formatTimeSpan(s.shift.startTime)} – ${formatTimeSpan(s.shift.endTime)}`
                          : '—'}
                      </td>
                      <td className="px-6 py-3 text-slate-500">{s.note ?? '—'}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </CardBody>
          </Card>
        )}
      </div>
    </div>
  )
}
