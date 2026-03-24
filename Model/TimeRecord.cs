using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Model
{
    // TimeRecord = Registro de ponto do dia
    // Um funcionário tem um registro por dia, com até 4 batidas:
    //   Entrada → Saída p/ almoço → Retorno do almoço → Saída
    [Table("time_record")]
    public class TimeRecord
    {
        [Key]
        public int? Id { get; private set; }

        public int EmployeeId { get; private set; }

        public DateOnly Date { get; private set; }

        // Cada batida é nullable porque pode não ter acontecido ainda
        public DateTime? ClockIn { get; private set; }       // Entrada
        public DateTime? BreakStart { get; private set; }    // Saiu pro almoço
        public DateTime? BreakEnd { get; private set; }      // Voltou do almoço
        public DateTime? ClockOut { get; private set; }      // Saída final

        // Minutos trabalhados calculados no momento da saída
        public int? WorkedMinutes { get; private set; }

        [ForeignKey("EmployeeId")]
        public Employee? Employee { get; private set; }

        public TimeRecord(int employeeId, DateOnly date)
        {
            EmployeeId = employeeId;
            Date = date;
        }

        // Métodos para registrar cada batida
        // Pensa como apertar o botão do relógio de ponto

        public void RegisterClockIn(DateTime time)
        {
            ClockIn = time;
        }

        public void RegisterBreakStart(DateTime time)
        {
            BreakStart = time;
        }

        public void RegisterBreakEnd(DateTime time)
        {
            BreakEnd = time;
        }

        public void RegisterClockOut(DateTime time)
        {
            ClockOut = time;
            WorkedMinutes = CalculateWorkedMinutes();
        }

        // Calcula quanto tempo o funcionário trabalhou no dia
        // Fórmula: (Saída - Entrada) - tempo de almoço
        private int CalculateWorkedMinutes()
        {
            if (ClockIn == null || ClockOut == null) return 0;

            var total = (int)(ClockOut.Value - ClockIn.Value).TotalMinutes;

            // Desconta o intervalo de almoço, se o funcionário registrou
            if (BreakStart != null && BreakEnd != null)
            {
                var breakMinutes = (int)(BreakEnd.Value - BreakStart.Value).TotalMinutes;
                total -= breakMinutes;
            }

            return total;
        }
    }
}
