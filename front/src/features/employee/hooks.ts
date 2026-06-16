import { useQuery, useMutation } from '@tanstack/react-query'
import { api } from '@/lib/api'
import { queryClient } from '@/lib/queryClient'
import { showToast } from '@/components/ui/Toast'
import type { TimeRecord, WorkSchedule, WorkShift } from '@/types/api.types'

// =============================================================
// Hooks do Portal do Empregado
// =============================================================

/** Busca o registro de ponto de hoje de um funcionário */
export function useTodayRecord(employeeId: number | null) {
  return useQuery<TimeRecord>({
    queryKey: ['timerecord-today', employeeId],
    queryFn: () =>
      api.get<TimeRecord>(`/timerecord/employee/${employeeId}/today`).then((r) => r.data),
    enabled: !!employeeId,
  })
}

/** Busca a escala de trabalho de um funcionário */
export function useSchedule(employeeId: number | null) {
  return useQuery<WorkSchedule[]>({
    queryKey: ['schedule', employeeId],
    queryFn: () =>
      api.get<WorkSchedule[]>(`/schedule/employee/${employeeId}`).then((r) => r.data),
    enabled: !!employeeId,
  })
}

/** Busca todos os turnos cadastrados */
export function useWorkShifts() {
  return useQuery<WorkShift[]>({
    queryKey: ['workshifts'],
    queryFn: () => api.get<WorkShift[]>('/workshift').then((r) => r.data),
  })
}

// Tipos de batida de ponto
export type ClockActionType = 'clockin' | 'breakstart' | 'breakend' | 'clockout'

const clockLabels: Record<ClockActionType, string> = {
  clockin:    'Entrada registrada!',
  breakstart: 'Saída para almoço registrada!',
  breakend:   'Retorno do almoço registrado!',
  clockout:   'Saída registrada!',
}

/** Mutation para bater ponto — chama a rota correta e atualiza o cache */
export function useClockAction(employeeId: number | null) {
  return useMutation({
    mutationFn: (type: ClockActionType) =>
      api.post(`/timerecord/employee/${employeeId}/${type}`).then((r) => r.data),

    onSuccess: (_data, type) => {
      // Invalida o cache do ponto de hoje para recarregar com os dados novos
      queryClient.invalidateQueries({ queryKey: ['timerecord-today', employeeId] })
      showToast(clockLabels[type], 'success')
    },

    onError: (err: Error) => {
      showToast(err.message || 'Erro ao registrar ponto.', 'error')
    },
  })
}
