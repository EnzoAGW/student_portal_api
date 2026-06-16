import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useMutation } from '@tanstack/react-query'
import { useNavigate } from 'react-router-dom'
import { api } from '@/lib/api'
import { useAuthStore } from '@/store/authStore'
import { getRoleFromToken, getUsernameFromToken } from '@/lib/utils'
import { showToast } from '@/components/ui/Toast'
import { Button } from '@/components/ui/Button'
import { Input } from '@/components/ui/Input'
import type { LoginResponse } from '@/types/api.types'

// Zod: schema de validação do formulário de login
// Define as regras do que é aceito antes de enviar para a API
const loginSchema = z.object({
  username: z.string().min(1, 'Informe o usuário'),
  password: z.string().min(1, 'Informe a senha'),
})

type LoginForm = z.infer<typeof loginSchema>

// Papéis que acessam o portal do empregado
const EMPLOYEE_ROLES = ['admin', 'rh', 'professor', 'secretaria']

export default function LoginPage() {
  const navigate = useNavigate()
  const setAuth = useAuthStore((s) => s.setAuth)

  // React Hook Form com validação via Zod
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginForm>({
    resolver: zodResolver(loginSchema),
  })

  // TanStack Query: mutation para fazer o login
  // useMutation é para operações que *modificam* dados (POST, PUT, DELETE)
  const loginMutation = useMutation({
    mutationFn: (data: LoginForm) =>
      api
        .post<LoginResponse>(`/auth?username=${data.username}&password=${data.password}`)
        .then((r) => r.data),

    onSuccess: (data) => {
      const role     = getRoleFromToken(data.token)
      const username = getUsernameFromToken(data.token) || ''

      // Salva no Zustand (que persiste no localStorage via middleware)
      setAuth(data.token, username, role)

      showToast('Login realizado com sucesso!', 'success')

      // Redireciona para o portal correto baseado no papel
      if (EMPLOYEE_ROLES.includes(role)) {
        navigate('/employee/ponto')
      } else {
        navigate('/student/enrollment')
      }
    },

    onError: (err: Error) => {
      showToast(err.message || 'Usuário ou senha inválidos.', 'error')
    },
  })

  return (
    <div className="min-h-screen bg-gradient-to-br from-primary to-primary-dark flex items-center justify-center p-4">
      <div className="w-full max-w-sm">
        {/* Card de login */}
        <div className="bg-white rounded-2xl shadow-2xl p-8">
          {/* Logo */}
          <div className="text-center mb-8">
            <div className="size-16 bg-primary-light rounded-2xl flex items-center justify-center mx-auto mb-4">
              <span className="text-3xl">🎓</span>
            </div>
            <h1 className="text-2xl font-bold text-slate-800">Portal Escolar</h1>
            <p className="text-slate-500 text-sm mt-1">Acesse sua conta</p>
          </div>

          {/* Formulário */}
          <form
            onSubmit={handleSubmit((data) => loginMutation.mutate(data))}
            className="space-y-4"
            noValidate
          >
            <Input
              label="Usuário"
              placeholder="Digite seu usuário"
              autoComplete="username"
              error={errors.username?.message}
              {...register('username')}
            />

            <Input
              label="Senha"
              type="password"
              placeholder="Digite sua senha"
              autoComplete="current-password"
              error={errors.password?.message}
              {...register('password')}
            />

            <Button
              type="submit"
              className="w-full mt-2"
              size="lg"
              loading={loginMutation.isPending}
            >
              Entrar
            </Button>
          </form>

          {/* Dica de usuários disponíveis */}
          <div className="mt-6 p-3 bg-slate-50 rounded-lg text-xs text-slate-500 space-y-1">
            <p className="font-medium text-slate-600">Usuários de teste:</p>
            <p><span className="font-mono">admin / 123</span> — Administrador</p>
            <p><span className="font-mono">professor / prof123</span> — Professor</p>
            <p><span className="font-mono">secretaria / sec123</span> — Secretaria</p>
            <p><span className="font-mono">rh / rh123</span> — RH</p>
          </div>
        </div>
      </div>
    </div>
  )
}
