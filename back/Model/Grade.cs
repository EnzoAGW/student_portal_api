using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    // Grade = Nota do aluno em uma matéria
    [Table("grade")]
    public class Grade
    {
        [Key]
        public int? Id { get; private set; }

        public int StudentId { get; private set; }

        public int SubjectId { get; private set; }

        // A nota em si (de 0 a 10)
        public decimal Value { get; private set; }

        // Tipo da avaliação: "Prova 1", "Prova 2", "Trabalho", etc.
        public string EvaluationType { get; private set; }

        public DateTime Date { get; private set; }

        [ForeignKey("StudentId")]
        public Student? Student { get; private set; }

        [ForeignKey("SubjectId")]
        public Subject? Subject { get; private set; }

        public Grade(int studentId, int subjectId, decimal value, string evaluationType, DateTime date)
        {
            StudentId = studentId;
            SubjectId = subjectId;
            Value = value;
            EvaluationType = evaluationType;
            Date = date;
        }
    }
}
