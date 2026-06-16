using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/schedule")]
    public class WorkScheduleController : ControllerBase
    {
        private readonly IWorkScheduleRepository _scheduleRepository;
        private readonly IWorkShiftRepository _shiftRepository;

        public WorkScheduleController(
            IWorkScheduleRepository scheduleRepository,
            IWorkShiftRepository shiftRepository)
        {
            _scheduleRepository = scheduleRepository;
            _shiftRepository = shiftRepository;
        }

        // POST /api/v1/schedule → Escalar funcionário em um dia
        [Authorize(Roles = "rh,admin")]
        [HttpPost]
        public IActionResult Add([FromBody] WorkScheduleViewModel scheduleView)
        {
            var shift = _shiftRepository.Get(scheduleView.WorkShiftId);
            if (shift == null)
                return NotFound(new { message = "Turno não encontrado." });

            // Evita escalar o mesmo funcionário duas vezes no mesmo dia
            if (_scheduleRepository.Exists(scheduleView.EmployeeId, scheduleView.Date))
                return Conflict(new { message = "Funcionário já está escalado nessa data." });

            var schedule = new WorkSchedule(
                scheduleView.EmployeeId,
                scheduleView.WorkShiftId,
                scheduleView.Date,
                scheduleView.Note
            );

            _scheduleRepository.Add(schedule);

            return Ok(new { message = $"Funcionário escalado para {scheduleView.Date:dd/MM/yyyy} no turno '{shift.Name}'." });
        }

        // GET /api/v1/schedule/employee/3 → Ver escala de um funcionário
        [Authorize]
        [HttpGet("employee/{employeeId}")]
        public IActionResult GetByEmployee(int employeeId)
        {
            var schedules = _scheduleRepository.GetByEmployee(employeeId);
            return Ok(schedules);
        }

        // GET /api/v1/schedule/date/2025-03-24 → Quem trabalha nesse dia?
        [Authorize]
        [HttpGet("date/{date}")]
        public IActionResult GetByDate(DateOnly date)
        {
            var schedules = _scheduleRepository.GetByDate(date);
            return Ok(schedules);
        }
    }
}
