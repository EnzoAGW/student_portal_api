using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Model;
using WebApplication1.ViewModel;

namespace WebApplication1.Tests.Controllers
{
    public class GradeControllerTests
    {
        private readonly Mock<IGradeRepository> _mockRepo = new();

        [Fact]
        public void Add_ValidGrade_Returns200()
        {
            _mockRepo.Setup(r => r.Add(It.IsAny<Grade>()));

            var controller = new GradeController(_mockRepo.Object);
            var viewModel = new GradeViewModel
            {
                StudentId      = 1,
                SubjectId      = 2,
                Value          = 8.5m,
                EvaluationType = "Prova 1",
                Date           = DateTime.Now
            };

            var result = controller.Add(viewModel);

            Assert.IsType<OkObjectResult>(result);
            _mockRepo.Verify(r => r.Add(It.IsAny<Grade>()), Times.Once);
        }

        [Fact]
        public void GetAverage_Returns200WithAverage()
        {
            _mockRepo.Setup(r => r.GetAverage(1, 2)).Returns(7.5m);

            var controller = new GradeController(_mockRepo.Object);

            var result = controller.GetAverage(studentId: 1, subjectId: 2);

            var ok = Assert.IsType<OkObjectResult>(result);
            // Verifica que o repositório foi consultado com os ids corretos
            _mockRepo.Verify(r => r.GetAverage(1, 2), Times.Once);
            Assert.NotNull(ok.Value);
        }

        [Fact]
        public void GetByStudent_Returns200WithGrades()
        {
            var grades = new List<Grade>
            {
                new Grade(studentId: 1, subjectId: 1, value: 9m,   evaluationType: "P1", date: DateTime.Now),
                new Grade(studentId: 1, subjectId: 2, value: 7.5m, evaluationType: "P2", date: DateTime.Now),
            };

            _mockRepo.Setup(r => r.GetByStudent(1)).Returns(grades);

            var controller = new GradeController(_mockRepo.Object);

            var result = controller.GetByStudent(studentId: 1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(grades, ok.Value);
        }
    }
}
