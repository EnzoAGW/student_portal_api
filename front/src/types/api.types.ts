// =============================================================
// Tipos que espelham os modelos do backend C#
// Cada interface aqui corresponde a uma classe Model do .NET
// =============================================================

// ---------- Auth ----------
export interface LoginResponse {
  token: string
}

// ---------- Portal do Aluno ----------

export interface Student {
  id: number
  name: string
  registration: string
  age?: number
  photo?: string
}

export interface Subject {
  id: number
  name: string
  code: string
  workload: number
}

// Matrícula do aluno em uma matéria
export interface StudentEnrollment {
  id: number
  studentId: number
  subjectId: number
  enrollmentDate: string
  subject?: Subject
}

// Nota individual (prova, trabalho, etc.)
export interface Grade {
  id: number
  studentId: number
  subjectId: number
  value: number
  evaluationType: string   // ex: "Prova 1", "Trabalho", "Final"
  date: string
  subject?: Subject
}

// Média do aluno numa matéria
export interface GradeAverage {
  studentId: number
  subjectId: number
  average: number
}

// Frequência
export interface AttendanceRecord {
  id: number
  studentId: number
  subjectId: number
  date: string
  present: boolean
}

// Resultado do cálculo de frequência (retornado pelo endpoint /percentage)
export interface AttendancePercentage {
  studentId: number
  subjectId: number
  presencePercentage: number
  status: string  // "Aprovado por frequência" | "Reprovado por falta"
}

// ---------- Portal do Empregado ----------

// Turno de trabalho (Manhã, Tarde, Noite, etc.)
export interface WorkShift {
  id: number
  name: string
  startTime: string   // "HH:mm:ss" vindo do TimeSpan do C#
  endTime: string
  expectedHours: number
}

// Escala: qual funcionário trabalha em qual dia com qual turno
export interface WorkSchedule {
  id: number
  employeeId: number
  date: string
  shiftId: number
  note?: string
  shift?: WorkShift
}

// Registro de ponto do dia
export interface TimeRecord {
  id: number
  employeeId: number
  date: string
  clockIn?: string      // DateTime? → ISO string ou null
  breakStart?: string
  breakEnd?: string
  clockOut?: string
  workedMinutes: number
}

// Funcionário
export interface Employee {
  id: number
  name: string
  age: number
  photo?: string
}
