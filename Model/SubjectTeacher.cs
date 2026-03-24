using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    // SubjectTeacher = "Qual professor leciona qual matéria?"
    // Ex: funcionário 7 (João) leciona Matemática no 1º semestre de 2025
    [Table("subject_teacher")]
    public class SubjectTeacher
    {
        [Key]
        public int? Id { get; private set; }

        // O professor é um Employee (funcionário)
        public int EmployeeId { get; private set; }

        public int SubjectId { get; private set; }

        public int Semester { get; private set; }

        public int Year { get; private set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; private set; }

        [ForeignKey("SubjectId")]
        public Subject? Subject { get; private set; }

        public SubjectTeacher(int employeeId, int subjectId, int semester, int year)
        {
            EmployeeId = employeeId;
            SubjectId = subjectId;
            Semester = semester;
            Year = year;
        }
    }
}
