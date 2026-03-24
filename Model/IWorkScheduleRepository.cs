namespace WebApplication1.Model
{
    public interface IWorkScheduleRepository
    {
        void Add(WorkSchedule schedule);

        // Escala de um funcionário (todos os dias futuros agendados)
        List<WorkSchedule> GetByEmployee(int employeeId);

        // Quem está escalado em uma data específica?
        List<WorkSchedule> GetByDate(DateOnly date);

        bool Exists(int employeeId, DateOnly date);
    }
}
