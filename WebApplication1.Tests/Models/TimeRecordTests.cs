using WebApplication1.Model;

namespace WebApplication1.Tests.Models
{
    // Testes do modelo TimeRecord
    // Aqui testamos a lógica de negócio pura, sem banco de dados, sem internet
    // É como testar uma calculadora: você só precisa dos números e das operações
    public class TimeRecordTests
    {
        [Fact]
        public void WorkedMinutes_WithBreak_DeductsBreakTime()
        {
            // Arrange = "preparar o cenário"
            var record = new TimeRecord(employeeId: 1, date: DateOnly.FromDateTime(DateTime.Today));

            var clockIn    = new DateTime(2025, 3, 24,  8,  0, 0); // 08:00
            var breakStart = new DateTime(2025, 3, 24, 12,  0, 0); // 12:00
            var breakEnd   = new DateTime(2025, 3, 24, 13,  0, 0); // 13:00
            var clockOut   = new DateTime(2025, 3, 24, 17,  0, 0); // 17:00

            // Act = "executar a ação"
            record.RegisterClockIn(clockIn);
            record.RegisterBreakStart(breakStart);
            record.RegisterBreakEnd(breakEnd);
            record.RegisterClockOut(clockOut);

            // Assert = "verificar o resultado"
            // 8h total - 1h almoço = 7h = 420 minutos
            Assert.Equal(480, record.WorkedMinutes); // 8h brutas
            // Na verdade: 17h - 8h = 9h bruto, - 1h almoço = 8h = 480min
        }

        [Fact]
        public void WorkedMinutes_WithoutBreak_CountsFullPeriod()
        {
            var record = new TimeRecord(employeeId: 1, date: DateOnly.FromDateTime(DateTime.Today));

            record.RegisterClockIn(new DateTime(2025, 3, 24, 8, 0, 0));
            record.RegisterClockOut(new DateTime(2025, 3, 24, 16, 0, 0));

            // 16h - 8h = 8h = 480 minutos (sem descontar almoço, não foi registrado)
            Assert.Equal(480, record.WorkedMinutes);
        }

        [Fact]
        public void WorkedMinutes_BeforeClockOut_IsNull()
        {
            var record = new TimeRecord(employeeId: 1, date: DateOnly.FromDateTime(DateTime.Today));

            record.RegisterClockIn(new DateTime(2025, 3, 24, 8, 0, 0));

            // Não bateu saída ainda → WorkedMinutes deve ser null
            Assert.Null(record.WorkedMinutes);
        }

        [Fact]
        public void WorkedMinutes_OvertimeScenario_IsCalculatedCorrectly()
        {
            var record = new TimeRecord(employeeId: 1, date: DateOnly.FromDateTime(DateTime.Today));

            // Funcionário fez hora extra: entrou 8h, saiu 19h, almoço 1h → 10h trabalhadas
            record.RegisterClockIn(new DateTime(2025, 3, 24, 8, 0, 0));
            record.RegisterBreakStart(new DateTime(2025, 3, 24, 12, 0, 0));
            record.RegisterBreakEnd(new DateTime(2025, 3, 24, 13, 0, 0));
            record.RegisterClockOut(new DateTime(2025, 3, 24, 19, 0, 0));

            // 11h bruto - 1h almoço = 10h = 600 minutos
            Assert.Equal(600, record.WorkedMinutes);
        }
    }
}
