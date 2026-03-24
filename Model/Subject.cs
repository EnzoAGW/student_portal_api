using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
 
    [Table("subject")]
    public class Subject
    {
        [Key]
        public int? Id { get; private set; }

        public string Name { get; private set; }

        public string? Description { get; private set; }

        // Carga horária: quantas horas tem essa matéria no semestre
        public int Workload { get; private set; }

        public Subject(string name, string? description, int workload)
        {
            Name = name;
            Description = description;
            Workload = workload;
        }
    }
}
