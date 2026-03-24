using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/grade")]
    public class GradeController : ControllerBase
    {
        private readonly IGradeRepository _gradeRepository;

        public GradeController(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository ?? throw new ArgumentNullException();
        }

        // POST /api/v1/grade → Lançar uma nota para um aluno
        // Só professor e admin podem lançar notas
        [Authorize(Roles = "professor,admin")]
        [HttpPost]
        public IActionResult Add([FromBody] GradeViewModel gradeView)
        {
            var grade = new Grade(
                gradeView.StudentId,
                gradeView.SubjectId,
                gradeView.Value,
                gradeView.EvaluationType,
                gradeView.Date
            );

            _gradeRepository.Add(grade);

            return Ok(new { message = "Nota lançada com sucesso!" });
        }

        // GET /api/v1/grade/student/5 → Ver todas as notas de um aluno
        [Authorize]
        [HttpGet("student/{studentId}")]
        public IActionResult GetByStudent(int studentId)
        {
            var grades = _gradeRepository.GetByStudent(studentId);
            return Ok(grades);
        }

        // GET /api/v1/grade/student/5/subject/2/average → Média do aluno na matéria
        [Authorize]
        [HttpGet("student/{studentId}/subject/{subjectId}/average")]
        public IActionResult GetAverage(int studentId, int subjectId)
        {
            var average = _gradeRepository.GetAverage(studentId, subjectId);
            return Ok(new { studentId, subjectId, average });
        }
    }
}
