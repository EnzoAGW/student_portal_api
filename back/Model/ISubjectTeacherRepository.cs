namespace WebApplication1.Model
{
    public interface ISubjectTeacherRepository
    {
        void Add(SubjectTeacher subjectTeacher);

        // Quais matérias esse professor leciona?
        List<SubjectTeacher> GetByEmployee(int employeeId);

        // Quais professores lecionam essa matéria?
        List<SubjectTeacher> GetBySubject(int subjectId);

        bool Exists(int employeeId, int subjectId, int semester, int year);
    }
}
