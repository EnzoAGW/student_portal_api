namespace WebApplication1.Model
{
    public interface IStudentEnrollmentRepository
    {
        void Add(StudentEnrollment enrollment);

        // Quais matérias esse aluno está matriculado?
        List<StudentEnrollment> GetByStudent(int studentId);

        // Quais alunos estão nessa matéria?
        List<StudentEnrollment> GetBySubject(int subjectId);

        // Verifica se o aluno já está matriculado nessa matéria (evita duplicatas)
        bool Exists(int studentId, int subjectId);
    }
}
