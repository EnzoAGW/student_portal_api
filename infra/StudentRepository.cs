using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ConnectionContext _context;

        public StudentRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(Student student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
        }

        public List<Student> Get(int pageNumber, int pageQtd)
        {
            return _context.Students
                .Skip(pageNumber * pageQtd)
                .Take(pageQtd)
                .ToList();
        }

        public Student? Get(int id)
        {
            return _context.Students.Find(id);
        }
    }
}
