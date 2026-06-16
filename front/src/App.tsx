import { createBrowserRouter, RouterProvider, Navigate } from 'react-router-dom'
import { ProtectedRoute } from '@/components/auth/ProtectedRoute'
import { ToastContainer } from '@/components/ui/Toast'

// Páginas
import LoginPage         from '@/features/auth/LoginPage'
import StudentLayout     from '@/features/student/StudentLayout'
import EnrollmentPage    from '@/features/student/pages/EnrollmentPage'
import GradesPage        from '@/features/student/pages/GradesPage'
import AttendancePage    from '@/features/student/pages/AttendancePage'
import EmployeeLayout    from '@/features/employee/EmployeeLayout'
import PontoPage         from '@/features/employee/pages/PontoPage'
import EscalaPage        from '@/features/employee/pages/EscalaPage'
import TurnosPage        from '@/features/employee/pages/TurnosPage'

// Papéis que podem acessar o portal do empregado
const EMPLOYEE_ROLES = ['admin', 'rh', 'professor', 'secretaria']

// createBrowserRouter é a forma moderna de definir rotas no React Router v7
// Permite lazy loading, loaders, actions e type-safety nos parâmetros
const router = createBrowserRouter([
  // Raíz → vai para o login
  {
    path: '/',
    element: <Navigate to="/login" replace />,
  },

  // Página de login (pública)
  {
    path: '/login',
    element: <LoginPage />,
  },

  // Portal do Aluno — protegido (qualquer usuário autenticado)
  {
    path: '/student',
    element: (
      <ProtectedRoute>
        <StudentLayout />
      </ProtectedRoute>
    ),
    children: [
      { index: true,           element: <Navigate to="enrollment" replace /> },
      { path: 'enrollment',   element: <EnrollmentPage /> },
      { path: 'grades',       element: <GradesPage /> },
      { path: 'attendance',   element: <AttendancePage /> },
    ],
  },

  // Portal do Empregado — protegido + verifica papel
  {
    path: '/employee',
    element: (
      <ProtectedRoute roles={EMPLOYEE_ROLES}>
        <EmployeeLayout />
      </ProtectedRoute>
    ),
    children: [
      { index: true,     element: <Navigate to="ponto" replace /> },
      { path: 'ponto',   element: <PontoPage /> },
      { path: 'escala',  element: <EscalaPage /> },
      { path: 'turnos',  element: <TurnosPage /> },
    ],
  },
])

export default function App() {
  return (
    <>
      <RouterProvider router={router} />
      {/* ToastContainer fica fora do router para aparecer em qualquer página */}
      <ToastContainer />
    </>
  )
}
