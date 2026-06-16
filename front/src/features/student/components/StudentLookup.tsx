import { useState } from 'react'
import { useStudentPortalStore } from '@/store/portalStore'
import { useStudent } from '../hooks'
import { Input } from '@/components/ui/Input'
import { Button } from '@/components/ui/Button'
import { Badge } from '@/components/ui/Badge'

// Componente de pesquisa de aluno — exibido no topo de todas as páginas do portal do aluno
// O usuário digita o ID do aluno e clica em "Buscar"
// O ID fica salvo no Zustand para que todas as abas usem o mesmo aluno
export function StudentLookup() {
  const { selectedStudentId, setSelectedStudentId } = useStudentPortalStore()
  const [inputId, setInputId] = useState(selectedStudentId?.toString() ?? '')

  const { data: student, isLoading, error } = useStudent(selectedStudentId)

  function handleSearch() {
    const id = parseInt(inputId)
    if (!isNaN(id) && id > 0) {
      setSelectedStudentId(id)
    }
  }

  return (
    <div className="bg-white border-b border-slate-200 px-6 py-4">
      <div className="flex items-center gap-3 flex-wrap">
        <div className="flex items-center gap-2">
          <Input
            placeholder="ID do aluno"
            type="number"
            value={inputId}
            onChange={(e) => setInputId(e.target.value)}
            onKeyDown={(e) => e.key === 'Enter' && handleSearch()}
            className="w-32"
          />
          <Button onClick={handleSearch} loading={isLoading} size="sm">
            Buscar
          </Button>
        </div>

        {/* Mostra o nome do aluno encontrado */}
        {student && (
          <div className="flex items-center gap-2">
            <span className="text-slate-400">→</span>
            <span className="font-semibold text-slate-800">{student.name}</span>
            <Badge variant="primary">Mat. {student.registration}</Badge>
          </div>
        )}

        {error && (
          <span className="text-sm text-danger">Aluno não encontrado.</span>
        )}
      </div>
    </div>
  )
}
