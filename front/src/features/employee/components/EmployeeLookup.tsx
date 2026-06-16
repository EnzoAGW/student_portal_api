import { useState } from 'react'
import { useEmployeePortalStore } from '@/store/portalStore'
import { Input } from '@/components/ui/Input'
import { Button } from '@/components/ui/Button'

// Componente de seleção de funcionário — similar ao StudentLookup
export function EmployeeLookup() {
  const { selectedEmployeeId, setSelectedEmployeeId } = useEmployeePortalStore()
  const [inputId, setInputId] = useState(selectedEmployeeId?.toString() ?? '')

  function handleLoad() {
    const id = parseInt(inputId)
    if (!isNaN(id) && id > 0) {
      setSelectedEmployeeId(id)
    }
  }

  return (
    <div className="bg-white border-b border-slate-200 px-6 py-4">
      <div className="flex items-center gap-3">
        <Input
          placeholder="ID do funcionário"
          type="number"
          value={inputId}
          onChange={(e) => setInputId(e.target.value)}
          onKeyDown={(e) => e.key === 'Enter' && handleLoad()}
          className="w-40"
        />
        <Button onClick={handleLoad} size="sm">
          Carregar
        </Button>
        {selectedEmployeeId && (
          <span className="text-sm text-slate-500">
            Funcionário <strong>#{selectedEmployeeId}</strong> selecionado
          </span>
        )}
      </div>
    </div>
  )
}
