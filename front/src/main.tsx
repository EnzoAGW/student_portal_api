import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { QueryClientProvider } from '@tanstack/react-query'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { queryClient } from '@/lib/queryClient'
import App from './App'
import './index.css'

// StrictMode: ativa verificações extras em desenvolvimento (não afeta produção)
// QueryClientProvider: disponibiliza o TanStack Query para toda a aplicação
// ReactQueryDevtools: painel de debug do cache — só aparece em desenvolvimento
createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <App />
      {import.meta.env.DEV && <ReactQueryDevtools initialIsOpen={false} />}
    </QueryClientProvider>
  </StrictMode>
)
