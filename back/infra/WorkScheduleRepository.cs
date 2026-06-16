using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class WorkScheduleRepository : IWorkScheduleRepository
    {
        private readonly ConnectionContext _context;

        public WorkScheduleRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(WorkSchedule schedule)
        {
            _context.WorkSchedules.Add(schedule);
            _context.SaveChanges();
        }

        public List<WorkSchedule> GetByEmployee(int employeeId)
        {
            return _context.WorkSchedules
                .Include(s => s.WorkShift)
                .Where(s => s.EmployeeId == employeeId)
                .OrderBy(s => s.Date)
                .ToList();
        }

        public List<WorkSchedule> GetByDate(DateOnly date)
        {
            return _context.WorkSchedules
                .Include(s => s.Employee)
                .Include(s => s.WorkShift)
                .Where(s => s.Date == date)
                .ToList();
        }

        public bool Exists(int employeeId, DateOnly date)
        {
            return _context.WorkSchedules
                .Any(s => s.EmployeeId == employeeId && s.Date == date);
        }
    }
}
