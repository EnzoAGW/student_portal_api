namespace WebApplication1.Model
{
    public interface IAttendanceRepository
    {
        void Add(Attendance attendance);

        // Todas as frequências de um aluno em uma matéria
        List<Attendance> GetByStudentAndSubject(int studentId, int subjectId);

        // Percentual de presença: ex → 75.0 significa 75% de presença
        double GetPresencePercentage(int studentId, int subjectId);
    }
}
