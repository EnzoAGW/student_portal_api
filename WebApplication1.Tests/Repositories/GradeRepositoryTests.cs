using Microsoft.EntityFrameworkCore;
using WebApplication1.infra;
using WebApplication1.Model;

namespace WebApplication1.Tests.Repositories
{
    // Testes de repositório usando banco de dados em memória
    // "Em memória" = o banco existe só enquanto o teste roda, depois some
    // É como fazer rascunho no papel: você usa, verifica, e joga fora
    public class GradeRepositoryTests
    {
        // Cria um banco em memória com um nome único para cada teste
        // Isso garante que os testes não interferem um no outro
        private static ConnectionContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ConnectionContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new ConnectionContext(options);
        }

        [Fact]
        public void GetAverage_WithGrades_ReturnsCorrectAverage()
        {
            using var context = CreateContext("GradeAverage_WithGrades");
            var repo = new GradeRepository(context);

            // Adiciona 3 notas para o aluno 1 na matéria 1
            repo.Add(new Grade(studentId: 1, subjectId: 1, value: 7.0m, evaluationType: "Prova 1", date: DateTime.Now));
            repo.Add(new Grade(studentId: 1, subjectId: 1, value: 8.0m, evaluationType: "Prova 2", date: DateTime.Now));
            repo.Add(new Grade(studentId: 1, subjectId: 1, value: 9.0m, evaluationType: "Trabalho", date: DateTime.Now));

            var average = repo.GetAverage(studentId: 1, subjectId: 1);

            // (7 + 8 + 9) / 3 = 8.0
            Assert.Equal(8.0m, average);
        }

        [Fact]
        public void GetAverage_WithNoGrades_ReturnsZero()
        {
            using var context = CreateContext("GradeAverage_Empty");
            var repo = new GradeRepository(context);

            var average = repo.GetAverage(studentId: 99, subjectId: 99);

            Assert.Equal(0m, average);
        }

        [Fact]
        public void GetByStudent_ReturnsOnlyThatStudentsGrades()
        {
            using var context = CreateContext("GradesByStudent");
            var repo = new GradeRepository(context);

            // Aluno 1 tem 2 notas, aluno 2 tem 1 nota
            repo.Add(new Grade(studentId: 1, subjectId: 1, value: 9m, evaluationType: "P1", date: DateTime.Now));
            repo.Add(new Grade(studentId: 1, subjectId: 2, value: 7m, evaluationType: "P1", date: DateTime.Now));
            repo.Add(new Grade(studentId: 2, subjectId: 1, value: 6m, evaluationType: "P1", date: DateTime.Now));

            var gradesStudent1 = repo.GetByStudent(studentId: 1);

            // Deve retornar só as 2 notas do aluno 1
            Assert.Equal(2, gradesStudent1.Count);
            Assert.All(gradesStudent1, g => Assert.Equal(1, g.StudentId));
        }
    }
}
