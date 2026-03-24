using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class TimeRecordRepository : ITimeRecordRepository
    {
        private readonly ConnectionContext _context;

        public TimeRecordRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(TimeRecord record)
        {
            _context.TimeRecords.Add(record);
            _context.SaveChanges();
        }

        public void Update(TimeRecord record)
        {
            _context.TimeRecords.Update(record);
            _context.SaveChanges();
        }

        public TimeRecord? GetToday(int employeeId)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            return _context.TimeRecords
                .FirstOrDefault(r => r.EmployeeId == employeeId && r.Date == today);
        }

        public List<TimeRecord> GetByMonth(int employeeId, int year, int month)
        {
            return _context.TimeRecords
                .Where(r => r.EmployeeId == employeeId
                         && r.Date.Year == year
                         && r.Date.Month == month)
                .OrderBy(r => r.Date)
                .ToList();
        }
    }
}
