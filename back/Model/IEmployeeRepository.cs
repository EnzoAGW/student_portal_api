namespace WebApplication1.Model
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);
        List<Employee> Get(int pageNumber, int pageQtd);
        Employee? Get(int id);
    }
}
