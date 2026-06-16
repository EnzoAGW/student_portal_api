using Microsoft.EntityFrameworkCore;
using WebApplication1.infra;
using WebApplication1.Model;

namespace WebApplication1.Tests.Repositories
{
    public class AttendanceRepositoryTests
    {
        private static ConnectionContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ConnectionContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new ConnectionContext(options);
        }

        [Fact]
        public void GetPresencePercentage_ThreeOutOfFour_Returns75()
        {
            using var context = CreateContext("Attendance_75Percent");
            var repo = new AttendanceRepository(context);

            var date = DateTime.Now;
            repo.Add(new Attendance(studentId: 1, subjectId: 1, date: date,            present: true));
            repo.Add(new Attendance(studentId: 1, subjectId: 1, date: date.AddDays(1), present: true));
            repo.Add(new Attendance(studentId: 1, subjectId: 1, date: date.AddDays(2), present: true));
            repo.Add(new Attendance(studentId: 1, subjectId: 1, date: date.AddDays(3), present: false)); // faltou

            var percentage = repo.GetPresencePercentage(studentId: 1, subjectId: 1);

            // 3 de 4 = 75%
            Assert.Equal(75.0, percentage);
        }

        [Fact]
        public void GetPresencePercentage_NoRecords_ReturnsZero()
        {
            using var context = CreateContext("Attendance_NoRecords");
            var repo = new AttendanceRepository(context);

            var percentage = repo.GetPresencePercentage(studentId: 99, subjectId: 99);

            Assert.Equal(0.0, percentage);
        }

        [Fact]
        public void GetPresencePercentage_AllPresent_Returns100()
        {
            using var context = CreateContext("Attendance_100Percent");
            var repo = new AttendanceRepository(context);

            var date = DateTime.Now;
            repo.Add(new Attendance(1, 1, date,            present: true));
            repo.Add(new Attendance(1, 1, date.AddDays(1), present: true));
            repo.Add(new Attendance(1, 1, date.AddDays(2), present: true));

            var percentage = repo.GetPresencePercentage(studentId: 1, subjectId: 1);

            Assert.Equal(100.0, percentage);
        }

        [Fact]
        public void GetByStudentAndSubject_ReturnsOrderedByDate()
        {
            using var context = CreateContext("Attendance_Ordered");
            var repo = new AttendanceRepository(context);

            var base_ = new DateTime(2025, 1, 1);
            repo.Add(new Attendance(1, 1, base_.AddDays(2), true));
            repo.Add(new Attendance(1, 1, base_.AddDays(0), true));
            repo.Add(new Attendance(1, 1, base_.AddDays(1), true));

            var records = repo.GetByStudentAndSubject(studentId: 1, subjectId: 1);

            // Deve vir ordenado por data crescente
            Assert.Equal(base_.AddDays(0), records[0].Date);
            Assert.Equal(base_.AddDays(1), records[1].Date);
            Assert.Equal(base_.AddDays(2), records[2].Date);
        }
    }
}
