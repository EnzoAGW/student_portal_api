using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/subject-teacher")]
    public class SubjectTeacherController : ControllerBase
    {
        private readonly ISubjectTeacherRepository _subjectTeacherRepository;
        private readonly ISubjectRepository _subjectRepository;

        public SubjectTeacherController(
            ISubjectTeacherRepository subjectTeacherRepository,
            ISubjectRepository subjectRepository)
        {
            _subjectTeacherRepository = subjectTeacherRepository;
            _subjectRepository = subjectRepository;
        }

        // POST /api/v1/subject-teacher → RH atribui professor a uma matéria
        [Authorize(Roles = "rh,admin")]
        [HttpPost]
        public IActionResult Add([FromBody] SubjectTeacherViewModel viewModel)
        {
            var subject = _subjectRepository.Get(viewModel.SubjectId);
            if (subject == null)
                return NotFound(new { message = "Matéria não encontrada." });

            if (_subjectTeacherRepository.Exists(viewModel.EmployeeId, viewModel.SubjectId, viewModel.Semester, viewModel.Year))
                return Conflict(new { message = "Este professor já está atribuído a esta matéria neste semestre." });

            var subjectTeacher = new SubjectTeacher(
                viewModel.EmployeeId,
                viewModel.SubjectId,
                viewModel.Semester,
                viewModel.Year
            );

            _subjectTeacherRepository.Add(subjectTeacher);

            return Ok(new { message = $"Professor atribuído à matéria '{subject.Name}' com sucesso!" });
        }

        // GET /api/v1/subject-teacher/employee/7 → Quais matérias esse professor leciona?
        [Authorize]
        [HttpGet("employee/{employeeId}")]
        public IActionResult GetByEmployee(int employeeId)
        {
            var assignments = _subjectTeacherRepository.GetByEmployee(employeeId);
            return Ok(assignments);
        }

        // GET /api/v1/subject-teacher/subject/2 → Quais professores lecionam essa matéria?
        [Authorize]
        [HttpGet("subject/{subjectId}")]
        public IActionResult GetBySubject(int subjectId)
        {
            var assignments = _subjectTeacherRepository.GetBySubject(subjectId);
            return Ok(assignments);
        }
    }
}
