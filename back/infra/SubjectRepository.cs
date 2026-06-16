using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ConnectionContext _context;

        public SubjectRepository(ConnectionContext context)
        {
            _context = context;
        }

        public void Add(Subject subject)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();
        }

        public List<Subject> GetAll()
        {
            return _context.Subjects.ToList();
        }

        public Subject? Get(int id)
        {
            return _context.Subjects.Find(id);
        }
    }
}
