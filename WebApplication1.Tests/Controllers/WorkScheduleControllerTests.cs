using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Tests.Controllers
{
    public class WorkScheduleControllerTests
    {
        private readonly Mock<IWorkScheduleRepository> _mockScheduleRepo = new();
        private readonly Mock<IWorkShiftRepository>    _mockShiftRepo    = new();

        [Fact]
        public void Add_ValidSchedule_Returns200()
        {
            var shift = new WorkShift("Manhã", TimeSpan.FromHours(7), TimeSpan.FromHours(15), 8);

            _mockShiftRepo.Setup(r => r.Get(1)).Returns(shift);
            _mockScheduleRepo.Setup(r => r.Exists(It.IsAny<int>(), It.IsAny<DateOnly>())).Returns(false);
            _mockScheduleRepo.Setup(r => r.Add(It.IsAny<WorkSchedule>()));

            var controller = new WorkScheduleController(_mockScheduleRepo.Object, _mockShiftRepo.Object);
            var viewModel = new WorkScheduleViewModel
            {
                EmployeeId  = 1,
                WorkShiftId = 1,
                Date        = new DateOnly(2025, 4, 1)
            };

            var result = controller.Add(viewModel);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void Add_DuplicateSchedule_Returns409Conflict()
        {
            var shift = new WorkShift("Manhã", TimeSpan.FromHours(7), TimeSpan.FromHours(15), 8);

            _mockShiftRepo.Setup(r => r.Get(1)).Returns(shift);
            // Simula que já existe escala nessa data
            _mockScheduleRepo.Setup(r => r.Exists(1, new DateOnly(2025, 4, 1))).Returns(true);

            var controller = new WorkScheduleController(_mockScheduleRepo.Object, _mockShiftRepo.Object);
            var viewModel = new WorkScheduleViewModel
            {
                EmployeeId  = 1,
                WorkShiftId = 1,
                Date        = new DateOnly(2025, 4, 1)
            };

            var result = controller.Add(viewModel);

            Assert.IsType<ConflictObjectResult>(result);
        }

        [Fact]
        public void Add_ShiftNotFound_Returns404()
        {
            // Turno não existe no banco
            _mockShiftRepo.Setup(r => r.Get(99)).Returns((WorkShift?)null);

            var controller = new WorkScheduleController(_mockScheduleRepo.Object, _mockShiftRepo.Object);
            var viewModel = new WorkScheduleViewModel
            {
                EmployeeId  = 1,
                WorkShiftId = 99,
                Date        = new DateOnly(2025, 4, 1)
            };

            var result = controller.Add(viewModel);

            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
