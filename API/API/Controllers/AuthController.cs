using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Identity;


using DataBase.Identity;
using DataBase.DTO;
using DataBase.Repositories;
using API.Tokens;
using API.ViewModels.Request;
using API.ViewModels.Responce;
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

        public AuthController(UserReposytory uReposytory, JwtFactory jwtFactory, TokenFactory tokenfactory, 
            UserManager<AppUser> uManager, EmailActions emailAct, IHttpContextAccessor httpContextAccessor)
        {
            _userReposytory = uReposytory;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenfactory;
            _uManager = uManager;
            _emailAct = emailAct;
            _httpContextAccessor = httpContextAccessor;

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
            var user = await _userReposytory.FindByName(responce.UserName);

            ContentResult json = new ContentResult();
            json.ContentType = "application/json";
            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };
            if (responce.Success)
            {
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
    }
}