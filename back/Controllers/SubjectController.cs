using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/subject")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectController(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository ?? throw new ArgumentNullException();
        }

        // POST /api/v1/subject → Cadastrar nova matéria
        // Roles = "secretaria,admin" → só esses papéis podem criar matérias
        // É como um crachá de acesso: só quem tem o crachá certo entra
        [Authorize(Roles = "secretaria,admin")]
        [HttpPost]
        public IActionResult Add([FromBody] SubjectViewModel subjectView)
        {
            var subject = new Subject(
                subjectView.Name,
                subjectView.Description,
                subjectView.Workload
            );

            _subjectRepository.Add(subject);

            return Ok(new { message = "Matéria cadastrada com sucesso!" });
        }

        // GET /api/v1/subject → Listar todas as matérias
        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var subjects = _subjectRepository.GetAll();
            return Ok(subjects);
        }

        // GET /api/v1/subject/3 → Buscar matéria pelo id
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var subject = _subjectRepository.Get(id);

            if (subject == null)
                return NotFound(new { message = "Matéria não encontrada." });

            return Ok(subject);
        }
    }
}
