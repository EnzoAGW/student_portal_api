import { Navigate } from 'react-router-dom'
import { useAuthStore } from '@/store/authStore'

interface ProtectedRouteProps {
  children: React.ReactNode
  // Se informado, verifica se o usuário tem algum dos papéis permitidos
  roles?: string[]
}

// Guarda de rota: se o usuário não estiver logado, redireciona para /login
// Se roles for informado, verifica se o papel do usuário tem permissão
export function ProtectedRoute({ children, roles }: ProtectedRouteProps) {
  const { token, role } = useAuthStore()

  // Não autenticado → vai para o login
  if (!token) {
    return <Navigate to="/login" replace />
  }

  // Papel sem permissão → volta para o login
  if (roles && role && !roles.includes(role)) {
    return <Navigate to="/login" replace />
  }

  return <>{children}</>
}
