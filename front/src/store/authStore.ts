import { create } from 'zustand'
import { persist } from 'zustand/middleware'

// Interface que descreve o que o store guarda e quais ações ele oferece
interface AuthState {
  token: string | null
  username: string | null
  role: string | null

  // Ação: salva os dados após o login
  setAuth: (token: string, username: string, role: string) => void

  // Ação: limpa tudo ao sair
  logout: () => void

  // Getter: verifica se o usuário está logado
  isAuthenticated: () => boolean
}

// Zustand: store de autenticação com persistência no localStorage
// "persist" faz com que os dados sobrevivam a um F5 (refresh da página)
export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      token: null,
      username: null,
      role: null,

      setAuth: (token, username, role) =>
        set({ token, username, role }),

      logout: () =>
        set({ token: null, username: null, role: null }),

      isAuthenticated: () => !!get().token,
    }),
    {
      name: 'portal-auth', // chave no localStorage
    }
  )
)
