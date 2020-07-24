using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using API.ViewModels.Request;
using API.ViewModels.Settings;
using API.Presenters;
using Core.Interfaces.Gateways.Reposytories;
using Core.Interfaces.UseCases;
using Core.DTO.UseCaseRequests;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserReposytory _uReposytory;
        private IRegisterUserUseCase _registerUserUseCase;
        private ILoginUserUseCase _loginUserUserCase;
        private IRefreshTokenUseCase _refreshTokenUseCase;
        private RegisterUserPresenter _registerUserPresenter;
        private LoginUserPresenter _loginUserPresenter;
        private RefreshTokenPresenter _refreshTokenPresenter;

        private AuthSettings _authSettings;

        public AuthController(IUserReposytory uReposytory, IRegisterUserUseCase registerUserUseCase, ILoginUserUseCase loginUserUseCase, IRefreshTokenUseCase refreshTokenUseCase,
            RegisterUserPresenter registerUserPresenter, LoginUserPresenter loginUserPresenter, RefreshTokenPresenter refreshTokenPresenter, IOptions<AuthSettings> authSettings)
        {
            _uReposytory = uReposytory;
            _registerUserUseCase = registerUserUseCase;
            _loginUserUserCase = loginUserUseCase;
            _refreshTokenUseCase = refreshTokenUseCase;
            _registerUserPresenter = registerUserPresenter;
            _loginUserPresenter = loginUserPresenter;
            _refreshTokenPresenter = refreshTokenPresenter;
            _authSettings = authSettings.Value;
        }
        [HttpGet("index")]
        public async Task<string> Index()
        {
            await Task.Delay(1);
            return "dsa";
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]API.ViewModels.Request.LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            await _loginUserUserCase.Handle(new Core.DTO.UseCaseRequests.LoginRequest(request.UserName, request.Password, ipAddress), _loginUserPresenter);
            return _loginUserPresenter.ContentResult;

        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _registerUserUseCase.Handle(new RegisterUserRequest(request.FirstName, request.SecondName, request.UserName, request.Email, request.Password), _registerUserPresenter);
            return _registerUserPresenter.ContentResult;

           
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody]RefreshRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _refreshTokenUseCase.Handle(new RefreshTokenRequest(request.AccessToken, request.RefreshToken, _authSettings.SecretKey), _refreshTokenPresenter);
            return _refreshTokenPresenter.ContentResult;
        }


    }
}