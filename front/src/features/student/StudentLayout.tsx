import { PortalLayout } from '@/components/layout/PortalLayout'

// Define os itens do menu lateral do Portal do Aluno
const NAV_ITEMS = [
  { to: '/student/enrollment', label: 'Minhas Matérias',  icon: '📚' },
  { to: '/student/grades',     label: 'Notas',            icon: '📊' },
  { to: '/student/attendance', label: 'Frequência',       icon: '📅' },
]

// Layout do Portal do Aluno — reutiliza o PortalLayout genérico
export default function StudentLayout() {
  return (
    <PortalLayout
      title="Portal do Aluno"
      navItems={NAV_ITEMS}
      accentColor="bg-primary"
    />
  )
}
