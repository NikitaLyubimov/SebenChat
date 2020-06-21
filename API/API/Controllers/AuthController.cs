using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using API.ViewModels.Request;
using API.Services.Interfaces;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _uService;

        public AuthController(IUserService uService)
        {
            _uService = uService;
        }
        [HttpGet("index")]
        public async Task<string> Index()
        {
            await Task.Delay(1);
            return "dsa";
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _uService.LoginUser(request.UserName, request.Password);
            return result;

        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _uService.RegisterUser(request.FirstName, request.SecondName, request.UserName, request.Email, request.Password);
            return result;

           
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody]RefreshRequest request)
        {
            var result = await _uService.Refresh(request.AccessToken, request.RefreshToken);

            return result;
        }


    }
}