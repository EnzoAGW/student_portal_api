using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly ConnectionContext _context;

        public AttendanceRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            _context.SaveChanges();
        }

        public List<Attendance> GetByStudentAndSubject(int studentId, int subjectId)
        {
            return _context.Attendances
                .Where(a => a.StudentId == studentId && a.SubjectId == subjectId)
                .OrderBy(a => a.Date)
                .ToList();
        }

        public double GetPresencePercentage(int studentId, int subjectId)
        {
            var records = _context.Attendances
                .Where(a => a.StudentId == studentId && a.SubjectId == subjectId)
                .ToList();

            if (records.Count == 0) return 0;

            var presences = records.Count(a => a.Present);
            return Math.Round((double)presences / records.Count * 100, 1);
        }
    }
}
