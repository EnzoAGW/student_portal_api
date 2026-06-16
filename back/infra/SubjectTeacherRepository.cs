using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class SubjectTeacherRepository : ISubjectTeacherRepository
    {
        private readonly ConnectionContext _context;

        public SubjectTeacherRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(SubjectTeacher subjectTeacher)
        {
            _context.SubjectTeachers.Add(subjectTeacher);
            _context.SaveChanges();
        }

        public List<SubjectTeacher> GetByEmployee(int employeeId)
        {
            return _context.SubjectTeachers
                .Include(st => st.Subject)
                .Where(st => st.EmployeeId == employeeId)
                .ToList();
        }

        public List<SubjectTeacher> GetBySubject(int subjectId)
        {
            return _context.SubjectTeachers
                .Include(st => st.Employee)
                .Where(st => st.SubjectId == subjectId)
                .ToList();
        }

        public bool Exists(int employeeId, int subjectId, int semester, int year)
        {
            return _context.SubjectTeachers
                .Any(st => st.EmployeeId == employeeId
                        && st.SubjectId == subjectId
                        && st.Semester == semester
                        && st.Year == year);
        }
    }
}
