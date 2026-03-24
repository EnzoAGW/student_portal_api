using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    // Attendance = Frequência/Chamada
    // Registra se o aluno estava presente ou faltou em um dia específico
    [Table("attendance")]
    public class Attendance
    {
        [Key]
        public int? Id { get; private set; }

        public int StudentId { get; private set; }

        public int SubjectId { get; private set; }

        // Data da aula
        public DateTime Date { get; private set; }

        // true = presente, false = faltou
        public bool Present { get; private set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; private set; }

        [ForeignKey("SubjectId")]
        public Subject? Subject { get; private set; }

        public Attendance(int studentId, int subjectId, DateTime date, bool present)
        {
            StudentId = studentId;
            SubjectId = subjectId;
            Date = date;
            Present = present;
        }
    }
}
