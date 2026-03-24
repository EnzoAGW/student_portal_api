using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class ConnectionContext : DbContext
    {
        // Construtor padrão — usado pelas migrations do EF Core
        public ConnectionContext() { }

        // Construtor com opções — usado pela injeção de dependência (e pelos testes!)
        // Pensa assim: quem cria o contexto decide qual banco usar.
        // No app real → PostgreSQL. Nos testes → banco em memória.
        public ConnectionContext(DbContextOptions<ConnectionContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<StudentEnrollment> StudentEnrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<WorkShift> WorkShifts { get; set; }
        public DbSet<WorkSchedule> WorkSchedules { get; set; }
        public DbSet<TimeRecord> TimeRecords { get; set; }
        public DbSet<SubjectTeacher> SubjectTeachers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // IsConfigured = alguém já passou as opções no construtor (ex: nos testes)
            // Só configura o PostgreSQL se ninguém já tiver configurado antes
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseNpgsql(
                    "Server=localhost;" +
                    "Port=5432;Database=employee_sample;" +
                    "User Id=postgres;" +
                    "Password=1234;");
        }
    }
}
