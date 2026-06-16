namespace WebApplication1.Model
{
    public interface ITimeRecordRepository
    {
        void Add(TimeRecord record);
        void Update(TimeRecord record);

        // Registro de hoje de um funcionário (pode ser null se ainda não bateu entrada)
        TimeRecord? GetToday(int employeeId);

        // Histórico de ponto do funcionário (por mês)
        List<TimeRecord> GetByMonth(int employeeId, int year, int month);
    }
}
