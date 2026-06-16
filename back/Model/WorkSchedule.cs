using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    // WorkSchedule = Escala de trabalho
    // Responde: "Quem trabalha, quando e em qual turno?"
    [Table("work_schedule")]
    public class WorkSchedule
    {
        [Key]
        public int? Id { get; private set; }

        public int EmployeeId { get; private set; }

        public int WorkShiftId { get; private set; }

        // O dia específico que o funcionário está escalado
        public DateOnly Date { get; private set; }

        public string? Note { get; private set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; private set; }

        [ForeignKey("WorkShiftId")]
        public WorkShift? WorkShift { get; private set; }

        public WorkSchedule(int employeeId, int workShiftId, DateOnly date, string? note)
        {
            EmployeeId = employeeId;
            WorkShiftId = workShiftId;
            Date = date;
            Note = note;
        }
    }
}
