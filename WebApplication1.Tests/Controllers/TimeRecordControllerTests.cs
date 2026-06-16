using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Model;

namespace WebApplication1.Tests.Controllers
{
    // Testes do fluxo de ponto — o mais rico em regras de negócio
    public class TimeRecordControllerTests
    {
        private readonly Mock<ITimeRecordRepository> _mockRepo = new();

        [Fact]
        public void ClockIn_FirstTimeToday_Returns200()
        {
            // Nenhum registro hoje ainda → deve permitir a entrada
            _mockRepo.Setup(r => r.GetToday(1)).Returns((TimeRecord?)null);
            _mockRepo.Setup(r => r.Add(It.IsAny<TimeRecord>()));

            var controller = new TimeRecordController(_mockRepo.Object);

            var result = controller.ClockIn(employeeId: 1);

            Assert.IsType<OkObjectResult>(result);
            _mockRepo.Verify(r => r.Add(It.IsAny<TimeRecord>()), Times.Once);
        }

        [Fact]
        public void ClockIn_AlreadyClockedIn_Returns409Conflict()
        {
            // Já existe um registro de hoje → não pode bater entrada de novo
            var existingRecord = new TimeRecord(employeeId: 1, date: DateOnly.FromDateTime(DateTime.Today));
            existingRecord.RegisterClockIn(DateTime.Now);

            _mockRepo.Setup(r => r.GetToday(1)).Returns(existingRecord);

            var controller = new TimeRecordController(_mockRepo.Object);

            var result = controller.ClockIn(employeeId: 1);

            // 409 Conflict = "já existe, não pode fazer de novo"
            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public void BreakStart_WithoutClockIn_Returns400()
        {
            // Sem registro de hoje → não pode sair pro almoço sem ter entrado
            _mockRepo.Setup(r => r.GetToday(1)).Returns((TimeRecord?)null);

            var controller = new TimeRecordController(_mockRepo.Object);

            var result = controller.BreakStart(employeeId: 1);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void BreakStart_WithClockIn_Returns200()
        {
            var record = new TimeRecord(employeeId: 1, date: DateOnly.FromDateTime(DateTime.Today));
            record.RegisterClockIn(DateTime.Now.AddHours(-4));

            _mockRepo.Setup(r => r.GetToday(1)).Returns(record);
            _mockRepo.Setup(r => r.Update(It.IsAny<TimeRecord>()));

            var controller = new TimeRecordController(_mockRepo.Object);

            var result = controller.BreakStart(employeeId: 1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void ClockOut_Returns200WithWorkedTime()
        {
            var record = new TimeRecord(employeeId: 1, date: DateOnly.FromDateTime(DateTime.Today));
            record.RegisterClockIn(DateTime.Now.AddHours(-8)); // entrou 8h atrás

            _mockRepo.Setup(r => r.GetToday(1)).Returns(record);
            _mockRepo.Setup(r => r.Update(It.IsAny<TimeRecord>()));

            var controller = new TimeRecordController(_mockRepo.Object);

            var result = controller.ClockOut(employeeId: 1);

            var ok = Assert.IsType<OkObjectResult>(result);
            // O resultado deve conter o campo "workedTime"
            var value = ok.Value!.ToString()!;
            Assert.Contains("workedTime", value);
        }

        [Fact]
        public void ClockOut_AlreadyClockedOut_Returns409()
        {
            var record = new TimeRecord(employeeId: 1, date: DateOnly.FromDateTime(DateTime.Today));
            record.RegisterClockIn(DateTime.Now.AddHours(-9));
            record.RegisterClockOut(DateTime.Now.AddHours(-1)); // já saiu

            _mockRepo.Setup(r => r.GetToday(1)).Returns(record);

            var controller = new TimeRecordController(_mockRepo.Object);

            var result = controller.ClockOut(employeeId: 1);

            Assert.IsType<ConflictObjectResult>(result);
        }
    }
}
