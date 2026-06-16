import { QueryClient } from '@tanstack/react-query'

// QueryClient é o "gerente" do TanStack Query
// Ele cuida do cache, refetch automático e estado das requisições
export const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      retry: 1,                  // tenta 1 vez se falhar, depois desiste
      staleTime: 1000 * 60 * 2,  // dados ficam "frescos" por 2 minutos
      refetchOnWindowFocus: false,
    },
  },
})
