import axios from 'axios'
import { useAuthStore } from '@/store/authStore'

// Instância do Axios já configurada com a URL base do backend
// Assim você não precisa digitar "http://localhost:5000/api/v1" em todo lugar
export const api = axios.create({
  // Caminho relativo — o Vite proxy repassa /api para o backend
  // Sem URL absoluta = sem CORS em desenvolvimento
  baseURL: '/api/v1',
  headers: { 'Content-Type': 'application/json' },
})

// Interceptor de requisição: injeta o token JWT em toda chamada automaticamente
// Pensa como: "antes de mandar cada pedido para o servidor, coloca o crachá"
api.interceptors.request.use((config) => {
  const token = useAuthStore.getState().token
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Interceptor de resposta: trata erros globais
// Se o servidor devolver 401 (não autorizado), desloga o usuário automaticamente
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      useAuthStore.getState().logout()
      window.location.href = '/login'
    }
    // Extrai a mensagem de erro do corpo da resposta para mostrar ao usuário
    const message =
      error.response?.data?.message ||
      error.response?.data ||
      error.message ||
      'Erro inesperado'
    return Promise.reject(new Error(String(message)))
  }
)
