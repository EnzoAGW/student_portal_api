import { create } from 'zustand'

// Store para guardar o ID do aluno selecionado no portal do aluno
// Necessário porque não temos login de aluno ainda — o usuário digita o ID manualmente
interface StudentPortalState {
  selectedStudentId: number | null
  setSelectedStudentId: (id: number | null) => void
}

export const useStudentPortalStore = create<StudentPortalState>()((set) => ({
  selectedStudentId: null,
  setSelectedStudentId: (id) => set({ selectedStudentId: id }),
}))

// Store para guardar o ID do funcionário no portal do empregado
interface EmployeePortalState {
  selectedEmployeeId: number | null
  setSelectedEmployeeId: (id: number | null) => void
}

export const useEmployeePortalStore = create<EmployeePortalState>()((set) => ({
  selectedEmployeeId: null,
  setSelectedEmployeeId: (id) => set({ selectedEmployeeId: id }),
}))
