using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/enrollment")]
    public class StudentEnrollmentController : ControllerBase
    {
        private readonly IStudentEnrollmentRepository _enrollmentRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ISubjectRepository _subjectRepository;

        public StudentEnrollmentController(
            IStudentEnrollmentRepository enrollmentRepository,
            IStudentRepository studentRepository,
            ISubjectRepository subjectRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _studentRepository = studentRepository;
            _subjectRepository = subjectRepository;
        }

        // POST /api/v1/enrollment → Matricular aluno em uma matéria
        // Só secretaria e admin podem matricular alunos
        [Authorize(Roles = "secretaria,admin")]
        [HttpPost]
        public IActionResult Add([FromBody] StudentEnrollmentViewModel enrollmentView)
        {
            // Verificamos se o aluno existe antes de matricular
            var student = _studentRepository.Get(enrollmentView.StudentId);
            if (student == null)
                return NotFound(new { message = "Aluno não encontrado." });

            // Verificamos se a matéria existe
            var subject = _subjectRepository.Get(enrollmentView.SubjectId);
            if (subject == null)
                return NotFound(new { message = "Matéria não encontrada." });

            // Verificamos se o aluno já está matriculado (não pode se matricular duas vezes)
            if (_enrollmentRepository.Exists(enrollmentView.StudentId, enrollmentView.SubjectId))
                return Conflict(new { message = "Aluno já matriculado nesta matéria." });

            var enrollment = new StudentEnrollment(
                enrollmentView.StudentId,
                enrollmentView.SubjectId,
                enrollmentView.Semester,
                enrollmentView.Year
            );

            _enrollmentRepository.Add(enrollment);

            return Ok(new { message = $"Aluno matriculado em '{subject.Name}' com sucesso!" });
        }

        // GET /api/v1/enrollment/student/5 → Ver todas as matérias de um aluno
        [Authorize]
        [HttpGet("student/{studentId}")]
        public IActionResult GetByStudent(int studentId)
        {
            var enrollments = _enrollmentRepository.GetByStudent(studentId);
            return Ok(enrollments);
        }

        // GET /api/v1/enrollment/subject/3 → Ver todos os alunos de uma matéria
        [Authorize]
        [HttpGet("subject/{subjectId}")]
        public IActionResult GetBySubject(int subjectId)
        {
            var enrollments = _enrollmentRepository.GetBySubject(subjectId);
            return Ok(enrollments);
        }
    }
}
