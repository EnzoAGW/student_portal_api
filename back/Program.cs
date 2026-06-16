using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using WebApplication1;
using WebApplication1.infra;
using WebApplication1.Model;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

// CORS: permite que o frontend (localhost:3000) chame o backend (localhost:5000)
// Sem isso, o browser bloqueia todas as requisições por segurança
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// AddDbContext registra o ConnectionContext no sistema de injeção de dependência.
// AddScoped = "usa o mesmo contexto durante toda a requisição HTTP"
// Isso é melhor que Transient para o DbContext.
builder.Services.AddDbContext<ConnectionContext>(options =>
    options.UseNpgsql(
        "Server=localhost;Port=5432;Database=employee_sample;User Id=postgres;Password=1234;"));

// Repositórios — o ASP.NET vai injetar o ConnectionContext automaticamente no construtor
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<IStudentEnrollmentRepository, StudentEnrollmentRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
builder.Services.AddScoped<IWorkShiftRepository, WorkShiftRepository>();
builder.Services.AddScoped<IWorkScheduleRepository, WorkScheduleRepository>();
builder.Services.AddScoped<ITimeRecordRepository, TimeRecordRepository>();
builder.Services.AddScoped<ISubjectTeacherRepository, SubjectTeacherRepository>();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT"
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var key = Encoding.ASCII.GetBytes(Key.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Em desenvolvimento, o redirect HTTPS quebra o CORS (a resposta 307 não leva os headers)
// Por isso desabilitamos o redirect apenas em dev — em produção ele volta a funcionar
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

app.UseCors("FrontendDev"); // ← precisa vir antes de Authentication
app.UseAuthentication(); // ← você estava sem este!
app.UseAuthorization();
app.MapControllers();
app.Run();