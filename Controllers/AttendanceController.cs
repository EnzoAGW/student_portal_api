using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/attendance")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceController(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        // POST /api/v1/attendance → Professor registra a chamada do dia
        // Só professor e admin podem registrar frequência
        [Authorize(Roles = "professor,admin")]
        [HttpPost]
        public IActionResult Add([FromBody] AttendanceViewModel attendanceView)
        {
            var attendance = new Attendance(
                attendanceView.StudentId,
                attendanceView.SubjectId,
                attendanceView.Date,
                attendanceView.Present
            );

            _attendanceRepository.Add(attendance);

            var status = attendanceView.Present ? "Presença" : "Falta";
            return Ok(new { message = $"{status} registrada com sucesso!" });
        }

        // GET /api/v1/attendance/student/5/subject/2 → Histórico de chamadas
        [Authorize]
        [HttpGet("student/{studentId}/subject/{subjectId}")]
        public IActionResult GetHistory(int studentId, int subjectId)
        {
            var records = _attendanceRepository.GetByStudentAndSubject(studentId, subjectId);
            return Ok(records);
        }

        // GET /api/v1/attendance/student/5/subject/2/percentage → % de presença
        // Ex: { "studentId": 5, "subjectId": 2, "presencePercentage": 75.0 }
        [Authorize]
        [HttpGet("student/{studentId}/subject/{subjectId}/percentage")]
        public IActionResult GetPercentage(int studentId, int subjectId)
        {
            var percentage = _attendanceRepository.GetPresencePercentage(studentId, subjectId);

            // 75% é o mínimo para não reprovar por falta (regra comum em faculdades)
            var approved = percentage >= 75.0;

            return Ok(new
            {
                studentId,
                subjectId,
                presencePercentage = percentage,
                status = approved ? "Aprovado por frequência" : "Reprovado por falta"
            });
        }
    }
}
