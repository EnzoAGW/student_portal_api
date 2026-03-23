using Microsoft.EntityFrameworkCore;
using WebApplication1.Model;

namespace WebApplication1.infra
{
    public class ConnectionContext:DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Server=localhost;" +
            "Port=5432;Database=employee_sample;" +
            "User Id=postgres;"+
            "Password=1234;"
            );


    }
}
