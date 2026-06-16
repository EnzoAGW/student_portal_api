using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class WorkShiftRepository : IWorkShiftRepository
    {
        private readonly ConnectionContext _context;

        public WorkShiftRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(WorkShift shift)
        {
            _context.WorkShifts.Add(shift);
            _context.SaveChanges();
        }

        public List<WorkShift> GetAll()
        {
            return _context.WorkShifts.ToList();
        }

        public WorkShift? Get(int id)
        {
            return _context.WorkShifts.Find(id);
        }
    }
}
