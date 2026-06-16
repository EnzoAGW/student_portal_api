import { useStudentPortalStore } from '@/store/portalStore'
import { useEnrollments, useAttendancePercentage } from '../hooks'
import { StudentLookup } from '../components/StudentLookup'
import { Card, CardHeader, CardBody } from '@/components/ui/Card'
import { Badge } from '@/components/ui/Badge'
import { Spinner } from '@/components/ui/Spinner'
import { EmptyState } from '@/components/ui/EmptyState'

// Sub-componente: frequência de uma matéria com barra de progresso visual
function AttendanceCard({ studentId, subjectId, subjectName }: {
  studentId: number
  subjectId: number
  subjectName: string
}) {
  const { data, isLoading } = useAttendancePercentage(studentId, subjectId)

  const pct  = data?.presencePercentage ?? 0
  // Cor: verde ≥ 75%, amarelo ≥ 50%, vermelho < 50%
  const color = pct >= 75 ? 'bg-success' : pct >= 50 ? 'bg-warning' : 'bg-danger'
  const variant = pct >= 75 ? 'success' : pct >= 50 ? 'warning' : 'danger'
  const approved = pct >= 75

  return (
    <Card>
      <CardHeader
        title={subjectName}
        action={
          data
            ? <Badge variant={approved ? 'success' : 'danger'}>
                {approved ? 'Aprovado' : 'Reprovado por falta'}
              </Badge>
            : null
        }
      />
      <CardBody>
        {isLoading ? (
          <Spinner size="sm" label="" />
        ) : data ? (
          <div className="space-y-2">
            {/* Barra de progresso */}
            <div className="flex items-center justify-between text-sm">
              <span className="text-slate-600">Presença</span>
              <span className={`font-bold ${approved ? 'text-success' : 'text-danger'}`}>
                {pct.toFixed(1)}%
              </span>
            </div>
            <div className="w-full bg-slate-100 rounded-full h-3">
              <div
                className={`h-3 rounded-full transition-all ${color}`}
                style={{ width: `${Math.min(pct, 100)}%` }}
              />
            </div>
            {/* Linha de mínimo exigido */}
            <div className="flex items-center justify-between text-xs text-slate-400">
              <span>Mínimo exigido: 75%</span>
              <Badge variant={variant}>{data.status}</Badge>
            </div>
          </div>
        ) : (
          <p className="text-sm text-slate-500">Sem registros de frequência.</p>
        )}
      </CardBody>
    </Card>
  )
}

export default function AttendancePage() {
  const { selectedStudentId } = useStudentPortalStore()
  const { data: enrollments, isLoading } = useEnrollments(selectedStudentId)

  return (
    <div>
      <StudentLookup />

      <div className="p-6">
        <h2 className="text-xl font-bold text-slate-800 mb-6">Frequência</h2>

        {!selectedStudentId ? (
          <EmptyState icon="🔍" title="Busque um aluno" description="Digite o ID do aluno acima para ver a frequência." />
        ) : isLoading ? (
          <Spinner className="py-16" label="Carregando frequência..." />
        ) : !enrollments?.length ? (
          <EmptyState icon="📅" title="Sem matérias" description="Este aluno não tem matérias matriculadas." />
        ) : (
          <div className="grid gap-4 sm:grid-cols-2">
            {enrollments.map((enrollment) => (
              <AttendanceCard
                key={enrollment.subjectId}
                studentId={selectedStudentId}
                subjectId={enrollment.subjectId}
                subjectName={enrollment.subject?.name ?? `Matéria #${enrollment.subjectId}`}
              />
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
