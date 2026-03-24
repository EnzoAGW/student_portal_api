using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    // WorkShift = Turno de trabalho
    // Ex: "Manhã" das 07:00 às 15:00, "Tarde" das 13:00 às 21:00
    [Table("work_shift")]
    public class WorkShift
    {
        [Key]
        public int? Id { get; private set; }

        public string Name { get; private set; }

        // TimeSpan representa um horário do dia (ex: 07:00:00)
        public TimeSpan StartTime { get; private set; }

        public TimeSpan EndTime { get; private set; }

        // Quantas horas esse turno espera que o funcionário trabalhe
        public int ExpectedHours { get; private set; }

        public WorkShift(string name, TimeSpan startTime, TimeSpan endTime, int expectedHours)
        {
            Name = name;
            StartTime = startTime;
            EndTime = endTime;
            ExpectedHours = expectedHours;
        }
    }
}
