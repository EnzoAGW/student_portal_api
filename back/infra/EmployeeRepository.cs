using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ConnectionContext _context;

        public EmployeeRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public List<Employee> Get(int pageNumber, int pageQtd)
        {
            return _context.Employees.Skip(pageNumber * pageQtd).Take(pageQtd).ToList();
        }

        public Employee? Get(int id)
        {
            return _context.Employees.Find(id);
        }
    }
}
