using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/timerecord")]
    public class TimeRecordController : ControllerBase
    {
        private readonly ITimeRecordRepository _timeRecordRepository;

        public TimeRecordController(ITimeRecordRepository timeRecordRepository)
        {
            _timeRecordRepository = timeRecordRepository;
        }

        // POST /api/v1/timerecord/employee/3/clockin → Bater ponto de ENTRADA
        [Authorize]
        [HttpPost("employee/{employeeId}/clockin")]
        public IActionResult ClockIn(int employeeId)
        {
            // Verifica se já bateu entrada hoje (não pode bater duas vezes)
            if (_timeRecordRepository.GetToday(employeeId) != null)
                return Conflict(new { message = "Entrada já registrada hoje." });

            var record = new TimeRecord(employeeId, DateOnly.FromDateTime(DateTime.Today));
            record.RegisterClockIn(DateTime.Now);
            _timeRecordRepository.Add(record);

            return Ok(new { message = "Entrada registrada!", time = DateTime.Now });
        }

        // POST /api/v1/timerecord/employee/3/breakstart → Saída para o almoço
        [Authorize]
        [HttpPost("employee/{employeeId}/breakstart")]
        public IActionResult BreakStart(int employeeId)
        {
            var record = _timeRecordRepository.GetToday(employeeId);

            if (record == null)
                return BadRequest(new { message = "Você precisa bater entrada primeiro." });

            if (record.ClockIn == null)
                return BadRequest(new { message = "Entrada não registrada." });

            if (record.BreakStart != null)
                return Conflict(new { message = "Saída para intervalo já registrada hoje." });

            record.RegisterBreakStart(DateTime.Now);
            _timeRecordRepository.Update(record);

            return Ok(new { message = "Saída para intervalo registrada!", time = DateTime.Now });
        }

        // POST /api/v1/timerecord/employee/3/breakend → Retorno do almoço
        [Authorize]
        [HttpPost("employee/{employeeId}/breakend")]
        public IActionResult BreakEnd(int employeeId)
        {
            var record = _timeRecordRepository.GetToday(employeeId);

            if (record == null || record.BreakStart == null)
                return BadRequest(new { message = "Saída para intervalo não registrada." });

            if (record.BreakEnd != null)
                return Conflict(new { message = "Retorno do intervalo já registrado hoje." });

            record.RegisterBreakEnd(DateTime.Now);
            _timeRecordRepository.Update(record);

            return Ok(new { message = "Retorno do intervalo registrado!", time = DateTime.Now });
        }

        // POST /api/v1/timerecord/employee/3/clockout → Bater ponto de SAÍDA
        [Authorize]
        [HttpPost("employee/{employeeId}/clockout")]
        public IActionResult ClockOut(int employeeId)
        {
            var record = _timeRecordRepository.GetToday(employeeId);

            if (record == null || record.ClockIn == null)
                return BadRequest(new { message = "Entrada não registrada." });

            if (record.ClockOut != null)
                return Conflict(new { message = "Saída já registrada hoje." });

            record.RegisterClockOut(DateTime.Now);
            _timeRecordRepository.Update(record);

            // Formata o resultado para mostrar as horas de forma legível
            var hours = record.WorkedMinutes / 60;
            var minutes = record.WorkedMinutes % 60;

            return Ok(new
            {
                message = "Saída registrada!",
                time = DateTime.Now,
                workedTime = $"{hours}h {minutes}min"
            });
        }

        // GET /api/v1/timerecord/employee/3/today → Ponto de hoje
        [Authorize]
        [HttpGet("employee/{employeeId}/today")]
        public IActionResult GetToday(int employeeId)
        {
            var record = _timeRecordRepository.GetToday(employeeId);

            if (record == null)
                return Ok(new { message = "Nenhuma batida registrada hoje." });

            return Ok(record);
        }

        // GET /api/v1/timerecord/employee/3/month?year=2025&month=3 → Espelho de ponto do mês
        [Authorize]
        [HttpGet("employee/{employeeId}/month")]
        public IActionResult GetMonth(int employeeId, int year, int month)
        {
            var records = _timeRecordRepository.GetByMonth(employeeId, year, month);

            // Calcula o total de horas trabalhadas no mês
            var totalMinutes = records.Sum(r => r.WorkedMinutes ?? 0);
            var totalHours = totalMinutes / 60;
            var remainingMinutes = totalMinutes % 60;

            return Ok(new
            {
                employeeId,
                year,
                month,
                totalWorked = $"{totalHours}h {remainingMinutes}min",
                records
            });
        }
    }
}
