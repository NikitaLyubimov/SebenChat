using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.IO;

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

        public AuthController(UserReposytory uReposytory, JwtFactory jwtFactory, TokenFactory tokenfactory, UserManager<AppUser> uManager)
        {
            _userReposytory = uReposytory;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenfactory;
            _uManager = uManager;

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
                        json.Content = JsonConvert.SerializeObject(new LoginResponce(acToken, refreshToken));

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

            if(responce.Success)
            {
                var emailConfToken = _tokenFactory.GenerateToken();

                EmailSettings settings = new EmailSettings();
                using (StreamReader reader = new StreamReader($@"{Environment.CurrentDirectory}\emailData.json"))
                {
                    string json = reader.ReadToEnd();
                    settings = JsonConvert.DeserializeObject<EmailSettings>(json);
                }
            }
        }
    }
}