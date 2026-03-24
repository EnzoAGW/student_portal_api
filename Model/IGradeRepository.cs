namespace WebApplication1.Model
{
    public interface IGradeRepository
    {
        void Add(Grade grade);

        // Busca todas as notas de um aluno em uma matéria específica
        List<Grade> GetByStudent(int studentId);

        // Média das notas de um aluno em uma matéria
        decimal GetAverage(int studentId, int subjectId);
    }
}
