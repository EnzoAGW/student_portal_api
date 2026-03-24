namespace WebApplication1.Model
{
    public interface ISubjectRepository
    {
        void Add(Subject subject);
        List<Subject> GetAll();
        Subject? Get(int id);
    }
}
