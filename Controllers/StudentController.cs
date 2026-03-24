using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers
{
    // [ApiController] = diz que essa classe é um "atendente" de API
    // [Route] = o "endereço" para chegar nesse atendente
    // Ex: POST http://localhost:5000/api/v1/student
    [ApiController]
    [Route("api/v1/student")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        // O ASP.NET injeta o repositório automaticamente (Injeção de Dependência)
        // Pensa assim: o atendente não precisa saber ONDE fica a gaveta,
        // alguém coloca a gaveta na mão dele
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException();
        }

        // POST /api/v1/student → Cadastrar novo aluno
        [Authorize]
        [HttpPost]
        public IActionResult Add([FromForm] StudentViewModel studentView)
        {
            string? filePath = null;

            // Se o aluno enviou uma foto, salvamos ela na pasta "storage"
            if (studentView.Photo != null)
            {
                filePath = Path.Combine("storage", studentView.Photo.FileName);
                using Stream fileStream = new FileStream(filePath, FileMode.Create);
                studentView.Photo.CopyTo(fileStream);
            }

            var student = new Student(
                studentView.Name,
                studentView.Email,
                studentView.Enrollment,
                studentView.Course,
                studentView.BirthDate,
                filePath
            );

            _studentRepository.Add(student);

            return Ok(new { message = "Aluno cadastrado com sucesso!" });
        }

        // GET /api/v1/student?pageNumber=0&pageQtd=10 → Listar alunos
        [Authorize]
        [HttpGet]
        public IActionResult Get(int pageNumber, int pageQtd)
        {
            var students = _studentRepository.Get(pageNumber, pageQtd);
            return Ok(students);
        }

        // GET /api/v1/student/5 → Buscar aluno pelo id
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var student = _studentRepository.Get(id);

            if (student == null)
                return NotFound(new { message = "Aluno não encontrado." });

            return Ok(student);
        }

        // POST /api/v1/student/5/download → Baixar foto do aluno
        [Authorize]
        [HttpPost("{id}/download")]
        public IActionResult DownloadPhoto(int id)
        {
            var student = _studentRepository.Get(id);

            if (student == null)
                return NotFound(new { message = "Aluno não encontrado." });

            if (student.Photo == null)
                return NotFound(new { message = "Este aluno não possui foto." });

            var dataBytes = System.IO.File.ReadAllBytes(student.Photo);
            return File(dataBytes, "image/png");
        }
    }
}
