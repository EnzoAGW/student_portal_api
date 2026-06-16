using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        // Usuários hardcoded por enquanto — futuramente virá do banco de dados
        // Pensa como uma lista de crachás: usuário → senha → papel
        private static readonly Dictionary<string, (string password, string role)> _users = new()
        {
            { "admin",      ("123",     "admin") },
            { "secretaria", ("sec123",  "secretaria") },
            { "professor",  ("prof123", "professor") },
            { "rh",         ("rh123",   "rh") },
        };

        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if (!_users.TryGetValue(username, out var user) || user.password != password)
                return Unauthorized(new { message = "Usuário ou senha inválidos." });

            var token = TokenService.GenerateToken(username, user.role);
            return Ok(token);
        }
    }
}