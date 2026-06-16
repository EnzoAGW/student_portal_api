import { useEffect, useState } from 'react'
import { cn } from '@/lib/utils'

export type ToastType = 'success' | 'error' | 'info' | 'warning'

export interface ToastMessage {
  id: string
  message: string
  type: ToastType
}

// Contexto global de toasts — gerenciado fora do React para permitir chamar
// showToast() de qualquer lugar sem depender de hooks
type Listener = (toasts: ToastMessage[]) => void

let toasts: ToastMessage[] = []
const listeners: Listener[] = []

function notify() {
  listeners.forEach((l) => l([...toasts]))
}

export function showToast(message: string, type: ToastType = 'info') {
  const id = Date.now().toString()
  toasts = [...toasts, { id, message, type }]
  notify()
  // Remove automaticamente após 3.5s
  setTimeout(() => {
    toasts = toasts.filter((t) => t.id !== id)
    notify()
  }, 3500)
}

const icons: Record<ToastType, string> = {
  success: '✓',
  error:   '✕',
  info:    'ℹ',
  warning: '⚠',
}

const styles: Record<ToastType, string> = {
  success: 'bg-success text-white',
  error:   'bg-danger text-white',
  info:    'bg-info text-white',
  warning: 'bg-warning text-white',
}

// Componente que renderiza todos os toasts na tela
export function ToastContainer() {
  const [activeToasts, setActiveToasts] = useState<ToastMessage[]>([])

  useEffect(() => {
    listeners.push(setActiveToasts)
    return () => {
      const idx = listeners.indexOf(setActiveToasts)
      listeners.splice(idx, 1)
    }
  }, [])

  if (activeToasts.length === 0) return null

  return (
    <div className="fixed bottom-6 right-6 z-50 flex flex-col gap-2">
      {activeToasts.map((toast) => (
        <div
          key={toast.id}
          className={cn(
            'flex items-center gap-3 px-4 py-3 rounded-lg shadow-lg text-sm font-medium',
            'animate-in slide-in-from-bottom-2 fade-in',
            styles[toast.type],
          )}
        >
          <span className="font-bold">{icons[toast.type]}</span>
          {toast.message}
        </div>
      ))}
    </div>
  )
}
