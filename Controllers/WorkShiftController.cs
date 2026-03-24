using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/workshift")]
    public class WorkShiftController : ControllerBase
    {
        private readonly IWorkShiftRepository _shiftRepository;

        public WorkShiftController(IWorkShiftRepository shiftRepository)
        {
            _shiftRepository = shiftRepository;
        }

        // POST /api/v1/workshift → Criar turno (só RH e admin)
        // Exemplo de body:
        // { "name": "Manhã", "startTime": "07:00", "endTime": "15:00", "expectedHours": 8 }
        [Authorize(Roles = "rh,admin")]
        [HttpPost]
        public IActionResult Add([FromBody] WorkShiftViewModel shiftView)
        {
            // TimeSpan.Parse converte "07:00" em um horário que o computador entende
            if (!TimeSpan.TryParse(shiftView.StartTime, out var start))
                return BadRequest(new { message = "Horário de início inválido. Use o formato HH:mm" });

            if (!TimeSpan.TryParse(shiftView.EndTime, out var end))
                return BadRequest(new { message = "Horário de fim inválido. Use o formato HH:mm" });

            var shift = new WorkShift(shiftView.Name, start, end, shiftView.ExpectedHours);
            _shiftRepository.Add(shift);

            return Ok(new { message = $"Turno '{shiftView.Name}' criado com sucesso!" });
        }

        // GET /api/v1/workshift → Listar todos os turnos
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var shifts = _shiftRepository.GetAll();
            return Ok(shifts);
        }
    }
}
