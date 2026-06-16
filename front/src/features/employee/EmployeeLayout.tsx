import { PortalLayout } from '@/components/layout/PortalLayout'

const NAV_ITEMS = [
  { to: '/employee/ponto',  label: 'Meu Ponto', icon: '⏰' },
  { to: '/employee/escala', label: 'Escala',    icon: '📆' },
  { to: '/employee/turnos', label: 'Turnos',    icon: '🔄' },
]

export default function EmployeeLayout() {
  return (
    <PortalLayout
      title="Portal do Empregado"
      navItems={NAV_ITEMS}
      accentColor="bg-emerald-600"
    />
  )
}
