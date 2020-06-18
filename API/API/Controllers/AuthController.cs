using System;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;



using DataBase.Identity;
using DataBase.DTO;
using DataBase.Repositories;
using API.Tokens;
using API.ViewModels.Request;
using API.ViewModels.Responce;
using API.ViewModels.Settings;
using API.ViewModels;
using API.Actions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private  UserReposytory _userReposytory;
        private JwtFactory _jwtFactory;
        private TokenFactory _tokenFactory;
        private UserManager<AppUser> _uManager;
        private EmailActions _emailAct;
        private IHttpContextAccessor _httpContextAccessor;
        private AuthSettings _authSettings;

        public AuthController(UserReposytory uReposytory, JwtFactory jwtFactory, TokenFactory tokenfactory, 
            UserManager<AppUser> uManager, EmailActions emailAct, IHttpContextAccessor httpContextAccessor, IOptions<AuthSettings> authSettings)
        {
            _userReposytory = uReposytory;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenfactory;
            _uManager = uManager;
            _emailAct = emailAct;
            _httpContextAccessor = httpContextAccessor;
            _authSettings = authSettings.Value;

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

            ContentResult json = new ContentResult();
            json.ContentType = "application/json";

            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };

            if(!string.IsNullOrEmpty(request.UserName) && !string.IsNullOrEmpty(request.Password))
            {
                var appUser = await _uManager.FindByNameAsync(request.UserName);
                var user = await _userReposytory.FindByName(request.UserName);

                if(appUser != null)
                {
                    if(await _uManager.CheckPasswordAsync(appUser, request.Password))
                    {
                        var refreshToken = _tokenFactory.GenerateToken();
                        user.AddRefreshToken(refreshToken, user.Id, Request.HttpContext.Connection.RemoteIpAddress.ToString());
                        await _userReposytory.Update(user);

                        AccessToken acToken = await _jwtFactory.GenerateEncodedToken(user.IdentityId, user.UserName);
                        json.StatusCode = (int)HttpStatusCode.OK;
                        json.Content = JsonConvert.SerializeObject(new LoginResponce(acToken, refreshToken), settings);

                        return json;
                        
                    }
                }
            }

            json.StatusCode = (int)HttpStatusCode.Unauthorized;
            json.Content = JsonConvert.SerializeObject(new[] { new Error("login_failure", "Invalid username or password") });
            return json;

        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody]RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var responce = await _userReposytory.Create(request.FirstName, request.SecondName, request.Email, request.UserName, request.Password);

            IdentityErrorDescriber d = new IdentityErrorDescriber();


            ContentResult json = new ContentResult();
            json.ContentType = "application/json";
            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };
            if (responce.Success)
            {
                var user = await _userReposytory.FindByName(responce.UserName);
                await _emailAct.SenMessage(_tokenFactory, request.Email,user.Id, _httpContextAccessor.HttpContext.Request.Host.Value);
                json.StatusCode = (int)HttpStatusCode.OK;
                json.Content = JsonConvert.SerializeObject(new RegisterResponce(responce.Id, true), settings);
                return json;
            }
            else
            {
                json.StatusCode = (int)HttpStatusCode.BadRequest;
                json.Content = JsonConvert.SerializeObject(new RegisterResponce(responce.Errors.Select(x => x.Description)), settings);
                return json;
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult> Refresh([FromBody]RefreshRequest request)
        {
            var cp = GetPrincipal(request.AccessToken);

            ContentResult json = new ContentResult();
            json.ContentType = "application/json";
            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };

            if (cp != null)
            {
                var id = cp.Claims.First(c => c.Type == "id");
                var user = await _userReposytory.GetByIdentityId(id.Value);

                

                if(user.HasValidRefreshTokens(request.RefreshToken))
                {
                    var jwtToken = await _jwtFactory.GenerateEncodedToken(user.IdentityId, user.UserName);
                    var refreshToken = _tokenFactory.GenerateToken();
                    user.RemoveRefreshToken(request.RefreshToken);
                    user.AddRefreshToken(refreshToken, user.Id, "");
                    await _userReposytory.Update(user);

                    json.StatusCode = (int)HttpStatusCode.OK;
                    json.Content = JsonConvert.SerializeObject(new RefreshResponce(jwtToken, refreshToken, true), settings);

                    return json;

                }
            }

            json.StatusCode = (int)HttpStatusCode.BadRequest;
            json.Content = JsonConvert.SerializeObject(new RefreshResponce(null, null, false, "Invalid token"), settings);
            return json;
        }

        private ClaimsPrincipal GetPrincipal(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SecretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken secToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out secToken);
            var jwtSecToken = secToken as JwtSecurityToken;

            if (jwtSecToken == null || !jwtSecToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}