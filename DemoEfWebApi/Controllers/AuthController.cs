using System.Threading.Tasks;
using System.Web.Http;
using DemoEfWebApi.Services.Interfaces;

namespace DemoEfWebApi.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        public class LoginRequest { public string Username { get; set; } public string Password { get; set; } }

        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null) return BadRequest("Missing data");
            var token = await _auth.LoginAsync(request.Username, request.Password);
            if (token == null) return Unauthorized();
            return Ok(new { token });
        }
    }
}
