using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    // Aqui descrevemos como é um Aluno no banco de dados
    // É como preencher um cadastro: o aluno tem nome, email, matrícula...
    [Table("student")]
    public class Student
    {
        [Key]
        public int? Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        // Matrícula: o número único que identifica o aluno na escola
        public string Enrollment { get; private set; }

        // Curso que o aluno faz (ex: Engenharia, Medicina...)
        public string Course { get; private set; }

        public DateTime BirthDate { get; private set; }

        public string? Photo { get; private set; }

        public Student(string name, string email, string enrollment, string course, DateTime birthDate, string? photo)
        {
            Name = name;
            Email = email;
            Enrollment = enrollment;
            Course = course;
            BirthDate = birthDate;
            Photo = photo;
        }
    }
}
