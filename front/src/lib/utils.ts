// =============================================================
// Funções utilitárias usadas em várias partes do app
// =============================================================

/**
 * Decodifica o payload de um JWT sem verificar a assinatura.
 * O JWT tem 3 partes separadas por ".": header.payload.signature
 * O payload é um JSON em base64 — aqui decodificamos para ler os dados.
 */
export function parseJwt(token: string): Record<string, string> {
  const base64 = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/')
  return JSON.parse(atob(base64))
}

/**
 * Extrai o papel (role) do usuário do token JWT.
 * O ASP.NET Core usa o claim completo abaixo para roles.
 */
export function getRoleFromToken(token: string): string {
  const payload = parseJwt(token)
  // ASP.NET usa este namespace longo para o claim de role
  return (
    payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ||
    payload['role'] ||
    ''
  )
}

/**
 * Extrai o nome do usuário do token JWT.
 */
export function getUsernameFromToken(token: string): string {
  const payload = parseJwt(token)
  return (
    payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ||
    payload['name'] ||
    ''
  )
}

/**
 * Converte minutos em formato legível: 487 → "8h 07m"
 */
export function formatMinutes(minutes: number): string {
  if (!minutes || minutes <= 0) return '—'
  const h = Math.floor(minutes / 60)
  const m = minutes % 60
  return `${h}h ${String(m).padStart(2, '0')}m`
}

/**
 * Formata uma string ISO de data/hora para horário BR: "2025-03-30T08:30:00" → "08:30"
 */
export function formatTime(isoString?: string): string {
  if (!isoString) return '—'
  const date = new Date(isoString)
  return date.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' })
}

/**
 * Formata uma string ISO de data para DD/MM/YYYY
 */
export function formatDate(isoString?: string): string {
  if (!isoString) return '—'
  const date = new Date(isoString)
  return date.toLocaleDateString('pt-BR')
}

/**
 * Converte "HH:mm:ss" (TimeSpan do C#) para "HH:mm"
 */
export function formatTimeSpan(timeSpan?: string): string {
  if (!timeSpan) return '—'
  return timeSpan.substring(0, 5) // pega só "HH:mm"
}

/**
 * Junta classes CSS condicionalmente.
 * Útil para combinar classes Tailwind dinamicamente.
 * Ex: cn('px-4', isActive && 'bg-primary', 'text-white')
 */
export function cn(...classes: (string | undefined | false | null)[]): string {
  return classes.filter(Boolean).join(' ')
}
