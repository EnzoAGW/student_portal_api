import { useStudentPortalStore } from '@/store/portalStore'
import { useEnrollments, useGrades, useGradeAverage } from '../hooks'
import { StudentLookup } from '../components/StudentLookup'
import { Card, CardHeader, CardBody } from '@/components/ui/Card'
import { Badge } from '@/components/ui/Badge'
import { Spinner } from '@/components/ui/Spinner'
import { EmptyState } from '@/components/ui/EmptyState'
import { formatDate } from '@/lib/utils'
import type { Grade } from '@/types/api.types'

// Sub-componente: exibe as notas e média de uma matéria
function SubjectGradeCard({ subjectId, subjectName, grades, studentId }: {
  subjectId: number
  subjectName: string
  grades: Grade[]
  studentId: number
}) {
  const { data: avgData } = useGradeAverage(studentId, subjectId)
  const average = avgData?.average ?? null

  // Cor da média: verde ≥ 6, amarelo ≥ 4, vermelho < 4
  const avgVariant = average === null ? 'default'
    : average >= 6 ? 'success'
    : average >= 4 ? 'warning'
    : 'danger'

  return (
    <Card>
      <CardHeader
        title={subjectName}
        action={
          average !== null
            ? <Badge variant={avgVariant} className="text-sm px-3 py-1">Média: {average.toFixed(1)}</Badge>
            : null
        }
      />
      <CardBody className="p-0">
        {grades.length === 0 ? (
          <p className="px-6 py-4 text-sm text-slate-500">Sem notas lançadas.</p>
        ) : (
          <table className="w-full text-sm">
            <thead className="bg-slate-50">
              <tr>
                <th className="text-left px-6 py-2 text-slate-600 font-medium">Avaliação</th>
                <th className="text-left px-6 py-2 text-slate-600 font-medium">Data</th>
                <th className="text-right px-6 py-2 text-slate-600 font-medium">Nota</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-slate-100">
              {grades.map((grade) => (
                <tr key={grade.id} className="hover:bg-slate-50 transition-colors">
                  <td className="px-6 py-3 text-slate-700">{grade.evaluationType}</td>
                  <td className="px-6 py-3 text-slate-500">{formatDate(grade.date)}</td>
                  <td className="px-6 py-3 text-right">
                    <Badge variant={grade.value >= 6 ? 'success' : grade.value >= 4 ? 'warning' : 'danger'}>
                      {grade.value.toFixed(1)}
                    </Badge>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </CardBody>
    </Card>
  )
}

export default function GradesPage() {
  const { selectedStudentId } = useStudentPortalStore()
  const { data: enrollments, isLoading: loadingEnrollments } = useEnrollments(selectedStudentId)
  const { data: grades, isLoading: loadingGrades } = useGrades(selectedStudentId)

  const isLoading = loadingEnrollments || loadingGrades

  // Agrupa as notas por matéria para exibição
  const gradesBySubject = (grades ?? []).reduce<Record<number, Grade[]>>((acc, grade) => {
    if (!acc[grade.subjectId]) acc[grade.subjectId] = []
    acc[grade.subjectId].push(grade)
    return acc
  }, {})

  return (
    <div>
      <StudentLookup />

      <div className="p-6">
        <h2 className="text-xl font-bold text-slate-800 mb-6">Notas</h2>

        {!selectedStudentId ? (
          <EmptyState icon="🔍" title="Busque um aluno" description="Digite o ID do aluno acima para ver suas notas." />
        ) : isLoading ? (
          <Spinner className="py-16" label="Carregando notas..." />
        ) : !enrollments?.length ? (
          <EmptyState icon="📊" title="Sem matérias" description="Este aluno não tem matérias matriculadas." />
        ) : (
          <div className="space-y-4">
            {enrollments.map((enrollment) => (
              <SubjectGradeCard
                key={enrollment.subjectId}
                subjectId={enrollment.subjectId}
                subjectName={enrollment.subject?.name ?? `Matéria #${enrollment.subjectId}`}
                grades={gradesBySubject[enrollment.subjectId] ?? []}
                studentId={selectedStudentId}
              />
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
