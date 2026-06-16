using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class StudentEnrollmentRepository : IStudentEnrollmentRepository
    {
        private readonly ConnectionContext _context;

        public StudentEnrollmentRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(StudentEnrollment enrollment)
        {
            _context.StudentEnrollments.Add(enrollment);
            _context.SaveChanges();
        }

        public List<StudentEnrollment> GetByStudent(int studentId)
        {
            return _context.StudentEnrollments
                .Include(e => e.Subject)
                .Where(e => e.StudentId == studentId)
                .ToList();
        }

        public List<StudentEnrollment> GetBySubject(int subjectId)
        {
            return _context.StudentEnrollments
                .Include(e => e.Student)
                .Where(e => e.SubjectId == subjectId)
                .ToList();
        }

        public bool Exists(int studentId, int subjectId)
        {
            return _context.StudentEnrollments
                .Any(e => e.StudentId == studentId && e.SubjectId == subjectId);
        }
    }
}
