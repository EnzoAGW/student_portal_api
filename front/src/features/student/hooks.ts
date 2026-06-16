import { useQuery } from '@tanstack/react-query'
import { api } from '@/lib/api'
import type { Student, StudentEnrollment, Grade, GradeAverage, AttendancePercentage } from '@/types/api.types'

// =============================================================
// Hooks do Portal do Aluno — cada hook busca um dado específico
// useQuery = busca dados (GET). Cuida de cache, loading e erro.
// queryKey = identificador único do cache. Se mudar o ID, refaz a busca.
// =============================================================

/** Busca os dados de um aluno pelo ID */
export function useStudent(id: number | null) {
  return useQuery<Student>({
    queryKey: ['student', id],
    queryFn: () => api.get<Student>(`/student/${id}`).then((r) => r.data),
    enabled: !!id, // só busca se id não for null/0
  })
}

/** Busca as matérias em que o aluno está matriculado */
export function useEnrollments(studentId: number | null) {
  return useQuery<StudentEnrollment[]>({
    queryKey: ['enrollments', studentId],
    queryFn: () =>
      api.get<StudentEnrollment[]>(`/enrollment/student/${studentId}`).then((r) => r.data),
    enabled: !!studentId,
  })
}

/** Busca todas as notas de um aluno (de todas as matérias) */
export function useGrades(studentId: number | null) {
  return useQuery<Grade[]>({
    queryKey: ['grades', studentId],
    queryFn: () =>
      api.get<Grade[]>(`/grade/student/${studentId}`).then((r) => r.data),
    enabled: !!studentId,
  })
}

/** Busca a média de um aluno em uma matéria específica */
export function useGradeAverage(studentId: number | null, subjectId: number) {
  return useQuery<GradeAverage>({
    queryKey: ['grade-average', studentId, subjectId],
    queryFn: () =>
      api
        .get<GradeAverage>(`/grade/student/${studentId}/subject/${subjectId}/average`)
        .then((r) => r.data),
    enabled: !!studentId && !!subjectId,
  })
}

/** Busca o percentual de frequência de um aluno em uma matéria */
export function useAttendancePercentage(studentId: number | null, subjectId: number) {
  return useQuery<AttendancePercentage>({
    queryKey: ['attendance-pct', studentId, subjectId],
    queryFn: () =>
      api
        .get<AttendancePercentage>(
          `/attendance/student/${studentId}/subject/${subjectId}/percentage`
        )
        .then((r) => r.data),
    enabled: !!studentId && !!subjectId,
  })
}
