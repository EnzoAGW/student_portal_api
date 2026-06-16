using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Tests.Controllers
{
    // Testes do StudentController usando Moq
    // Moq = "simulador de repositório"
    // Em vez de usar o banco de dados real, criamos um repositório falso
    // que se comporta exatamente como queremos para cada teste
    public class StudentControllerTests
    {
        // Mock = um dublê de filme: parece o repositório real, mas é controlado por nós
        private readonly Mock<IStudentRepository> _mockRepo = new();

        [Fact]
        public void GetById_ExistingStudent_Returns200WithStudent()
        {
            // Arrange: configura o mock para retornar um aluno quando chamado com id=1
            var student = new Student("Ana", "ana@email.com", "2025001", "Engenharia",
                new DateTime(2000, 5, 10), null);

            _mockRepo.Setup(r => r.Get(1)).Returns(student);

            var controller = new StudentController(_mockRepo.Object);

            // Act
            var result = controller.GetById(1);

            // Assert: deve retornar 200 OK com o aluno
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(student, ok.Value);
        }

        [Fact]
        public void GetById_NonExistingStudent_Returns404()
        {
            // Configura o mock para retornar null (aluno não existe)
            _mockRepo.Setup(r => r.Get(99)).Returns((Student?)null);

            var controller = new StudentController(_mockRepo.Object);

            var result = controller.GetById(99);

            // Deve retornar 404 Not Found
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void Get_ReturnsPagedList()
        {
            var students = new List<Student>
            {
                new Student("Ana",   "ana@email.com",   "001", "Eng", new DateTime(2000,1,1), null),
                new Student("Bruno", "bruno@email.com", "002", "Med", new DateTime(2001,2,2), null),
            };

            _mockRepo.Setup(r => r.Get(0, 10)).Returns(students);

            var controller = new StudentController(_mockRepo.Object);

            var result = controller.Get(pageNumber: 0, pageQtd: 10);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(students, ok.Value);
        }

        [Fact]
        public void Add_WithoutPhoto_Returns200()
        {
            _mockRepo.Setup(r => r.Add(It.IsAny<Student>())); // aceita qualquer Student

            var controller = new StudentController(_mockRepo.Object);
            var viewModel = new StudentViewModel
            {
                Name       = "Carlos",
                Email      = "carlos@email.com",
                Enrollment = "2025003",
                Course     = "Direito",
                BirthDate  = new DateTime(1999, 3, 15),
                Photo      = null
            };

            var result = controller.Add(viewModel);

            Assert.IsType<OkObjectResult>(result);
            // Verifica que o repositório foi chamado exatamente 1 vez
            _mockRepo.Verify(r => r.Add(It.IsAny<Student>()), Times.Once);
        }
    }
}
