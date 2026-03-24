namespace WebApplication1.Model
{
    // Interface = um "contrato"
    // Aqui dizemos: "qualquer repositório de aluno PRECISA saber fazer essas coisas"
    public interface IStudentRepository
    {
        void Add(Student student);
        List<Student> Get(int pageNumber, int pageQtd);
        Student? Get(int id);
    }
}
