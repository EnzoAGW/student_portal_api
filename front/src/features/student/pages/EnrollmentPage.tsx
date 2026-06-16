import { useStudentPortalStore } from '@/store/portalStore'
import { useEnrollments } from '../hooks'
import { StudentLookup } from '../components/StudentLookup'
import { Card, CardBody } from '@/components/ui/Card'
import { Badge } from '@/components/ui/Badge'
import { Spinner } from '@/components/ui/Spinner'
import { EmptyState } from '@/components/ui/EmptyState'
import { formatDate } from '@/lib/utils'

export default function EnrollmentPage() {
  const { selectedStudentId } = useStudentPortalStore()
  const { data: enrollments, isLoading } = useEnrollments(selectedStudentId)

  return (
    <div>
      <StudentLookup />

      <div className="p-6">
        <h2 className="text-xl font-bold text-slate-800 mb-6">Minhas Matérias</h2>

        {!selectedStudentId ? (
          <EmptyState icon="🔍" title="Busque um aluno" description="Digite o ID do aluno acima para ver suas matérias." />
        ) : isLoading ? (
          <Spinner className="py-16" label="Carregando matérias..." />
        ) : !enrollments?.length ? (
          <EmptyState icon="📚" title="Nenhuma matéria" description="Este aluno não está matriculado em nenhuma matéria." />
        ) : (
          <div className="grid gap-4 sm:grid-cols-2 lg:grid-cols-3">
            {enrollments.map((enrollment) => (
              <Card key={enrollment.id}>
                <CardBody>
                  <div className="flex items-start justify-between">
                    <div>
                      <p className="font-semibold text-slate-800">
                        {enrollment.subject?.name ?? `Matéria #${enrollment.subjectId}`}
                      </p>
                      {enrollment.subject?.code && (
                        <p className="text-sm text-slate-500 mt-0.5">{enrollment.subject.code}</p>
                      )}
                    </div>
                    <Badge variant="primary">Ativo</Badge>
                  </div>

                  <div className="mt-3 pt-3 border-t border-slate-100 flex items-center justify-between text-xs text-slate-500">
                    <span>Carga: {enrollment.subject?.workload ?? '—'}h</span>
                    <span>Matrícula: {formatDate(enrollment.enrollmentDate)}</span>
                  </div>
                </CardBody>
              </Card>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
