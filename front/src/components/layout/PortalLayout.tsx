import { NavLink, Outlet } from 'react-router-dom'
import { useAuthStore } from '@/store/authStore'
import { cn } from '@/lib/utils'

export interface NavItem {
  to: string
  label: string
  icon: string
}

interface PortalLayoutProps {
  title: string          // ex: "Portal do Aluno"
  navItems: NavItem[]
  accentColor?: string   // classe Tailwind para a cor do destaque lateral
}

// Layout padrão dos portais: sidebar à esquerda + conteúdo à direita
// Funciona tanto para o portal do aluno quanto do empregado
export function PortalLayout({ title, navItems, accentColor = 'bg-primary' }: PortalLayoutProps) {
  const { username, role, logout } = useAuthStore()

  return (
    <div className="flex h-screen overflow-hidden bg-bg">
      {/* ===== SIDEBAR ===== */}
      <aside className="w-60 flex flex-col bg-sidebar text-white shrink-0">
        {/* Logo / título do portal */}
        <div className="px-5 py-6 border-b border-white/10">
          <div className="flex items-center gap-3">
            <div className={cn('size-8 rounded-lg flex items-center justify-center text-base', accentColor)}>
              🎓
            </div>
            <div>
              <p className="text-xs text-slate-400">Portal Escolar</p>
              <p className="text-sm font-semibold">{title}</p>
            </div>
          </div>
        </div>

        {/* Menu de navegação */}
        <nav className="flex-1 px-3 py-4 space-y-1">
          {navItems.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              className={({ isActive }) =>
                cn(
                  'flex items-center gap-3 px-3 py-2.5 rounded-lg text-sm transition-colors',
                  isActive
                    ? 'bg-white/10 text-white font-medium'
                    : 'text-slate-400 hover:bg-white/5 hover:text-white',
                )
              }
            >
              <span className="text-base">{item.icon}</span>
              {item.label}
            </NavLink>
          ))}
        </nav>

        {/* Rodapé com usuário + botão de logout */}
        <div className="px-4 py-4 border-t border-white/10">
          <div className="flex items-center gap-3 mb-3">
            <div className="size-8 rounded-full bg-primary/30 flex items-center justify-center text-sm font-bold">
              {username?.[0]?.toUpperCase() ?? 'U'}
            </div>
            <div className="overflow-hidden">
              <p className="text-sm font-medium truncate">{username}</p>
              <p className="text-xs text-slate-400 capitalize">{role}</p>
            </div>
          </div>
          <button
            onClick={logout}
            className="w-full flex items-center gap-2 px-3 py-2 rounded-lg text-sm text-slate-400
                       hover:bg-white/5 hover:text-white transition-colors"
          >
            <span>←</span> Sair
          </button>
        </div>
      </aside>

      {/* ===== CONTEÚDO PRINCIPAL ===== */}
      {/* Outlet renderiza a página filha baseada na rota atual */}
      <main className="flex-1 overflow-y-auto">
        <Outlet />
      </main>
    </div>
  )
}
