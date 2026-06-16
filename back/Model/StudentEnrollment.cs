using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    // StudentEnrollment = "O aluno X está matriculado na matéria Y"
    // É como a lista de chamada: conecta o aluno à matéria
    [Table("student_enrollment")]
    public class StudentEnrollment
    {
        [Key]
        public int? Id { get; private set; }

        // Qual aluno?
        public int StudentId { get; private set; }

        // Em qual matéria?
        public int SubjectId { get; private set; }

        // Em qual semestre? (1 ou 2)
        public int Semester { get; private set; }

        // Em qual ano? (ex: 2025)
        public int Year { get; private set; }

        // Navigation properties: permitem acessar os dados completos
        // Pensa assim: ao invés de só ter o "id" do aluno, você pode pegar o aluno inteiro
        [ForeignKey("StudentId")]
        public Student? Student { get; private set; }

        [ForeignKey("SubjectId")]
        public Subject? Subject { get; private set; }

        public StudentEnrollment(int studentId, int subjectId, int semester, int year)
        {
            StudentId = studentId;
            SubjectId = subjectId;
            Semester = semester;
            Year = year;
        }
    }
}
