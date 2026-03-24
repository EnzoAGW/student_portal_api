using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class GradeRepository : IGradeRepository
    {
        private readonly ConnectionContext _context;

        public GradeRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(Grade grade)
        {
            _context.Grades.Add(grade);
            _context.SaveChanges();
        }

        public List<Grade> GetByStudent(int studentId)
        {
            return _context.Grades
                .Where(g => g.StudentId == studentId)
                .ToList();
        }

        public decimal GetAverage(int studentId, int subjectId)
        {
            // ToList() traz os dados para a memória antes de calcular a média
            // Isso evita problemas de tradução SQL e funciona no banco InMemory (testes)
            var values = _context.Grades
                .Where(g => g.StudentId == studentId && g.SubjectId == subjectId)
                .Select(g => g.Value)
                .ToList();

            return values.Count == 0 ? 0 : values.Average();
        }
    }
}
