using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : Controller
    {
        [HttpPost]
        public IActionResult Auth(string username, string password)
        {
            if (username == "admin" && password == "123")
            {
                var token = TokenService.GenerateToken(new Model.Employee("admin", 0, null));
                return Ok(token);
            }

            return BadRequest("Caba otário");
        }
    }
}