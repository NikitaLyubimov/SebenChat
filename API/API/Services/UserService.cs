using System;
using System.Net;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using API.Actions;
using API.Presenters;
using API.Services.Interfaces;
using API.Tokens;
using API.ViewModels;
using API.ViewModels.Settings;
using DataBase.Identity;
using DataBase.Repositories;


using API.ViewModels.Responce;
using DataBase.DTO;




namespace API.Services
{
    public class UserService : IUserService
    {
        private UserReposytory _userReposytory;
        private JwtFactory _jwtFactory;
        private TokenFactory _tokenFactory;
        private UserManager<AppUser> _uManager;
        private EmailActions _emailAct;
        private IHttpContextAccessor _httpContextAccessor;
        private AuthSettings _authSettings;

        public UserService(UserManager<AppUser> uManager, UserReposytory uReposytory, JwtFactory jwtFactory, TokenFactory tokenFactory,
            EmailActions emailAct, IHttpContextAccessor httpContextAccessor, IOptions<AuthSettings> authSettings)
        {
            _userReposytory = uReposytory;
            _jwtFactory = jwtFactory;
            _tokenFactory = tokenFactory;
            _uManager = uManager;
            _emailAct = emailAct;
            _httpContextAccessor = httpContextAccessor;
            _authSettings = authSettings.Value;

        }

        /// <summary>
        /// Log in user in system
        /// </summary>
        /// <param name="username">User's username</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        public async Task<JsonContentResult> LoginUser(string username, string password)
        {

            JsonContentResult json = new JsonContentResult();

            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                var appUser = await _uManager.FindByNameAsync(username);
                var user = await _userReposytory.FindByName(username);

                if (appUser != null)
                {
                    if (await _uManager.CheckPasswordAsync(appUser, password))
                    {
                        var refreshToken = _tokenFactory.GenerateToken();
                        user.AddRefreshToken(refreshToken, user.Id, _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());
                        await _userReposytory.Update(user);

                        AccessToken acToken = await _jwtFactory.GenerateEncodedToken(user.IdentityId, user.UserName);
                        json.StatusCode = (int)HttpStatusCode.OK;
                        json.Content = JsonConvert.SerializeObject(new LoginResponce(null, true, acToken, refreshToken), settings);

                        return json;

                    }
                }
            }

                json.StatusCode = (int)HttpStatusCode.Unauthorized;
                var error = new Error("login_failure", "Invalid username or password");
                json.Content = JsonConvert.SerializeObject(new LoginResponce(error, false), settings);
                return json;
            }

        /// <summary>
        /// Registers user in system
        /// </summary>
        /// <param name="firstName">User's firstname</param>
        /// <param name="secondName">User's secondname</param>
        /// <param name="username">User's username</param>
        /// <param name="email">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        public async Task<JsonContentResult> RegisterUser(string firstName, string secondName, string username, string email, string password)
        {
            var responce = await _userReposytory.Create(firstName, secondName, email, username, password);

            IdentityErrorDescriber d = new IdentityErrorDescriber();


            JsonContentResult json = new JsonContentResult();

            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };
            if (responce.Success)
            {
                var user = await _userReposytory.FindByName(responce.UserName);
                await _emailAct.SenMessage(_tokenFactory, email, user.Id, _httpContextAccessor.HttpContext.Request.Host.Value);
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


        /// <summary>
        /// Generate new token pair when old accessToken is expired
        /// </summary>
        /// <param name="accessToken">old access token</param>
        /// <param name="refreshToken">old refresh token</param>
        /// <returns></returns>
        public async Task<JsonContentResult> Refresh(string accessToken, string refreshToken)
        {
            var cp = GetPrincipal(accessToken);

            JsonContentResult json = new JsonContentResult();
           
            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };

            if (cp != null)
            {
                var id = cp.Claims.First(c => c.Type == "id");
                var user = await _userReposytory.GetByIdentityId(id.Value);



                if (user.HasValidRefreshTokens(refreshToken))
                {
                    var jwtToken = await _jwtFactory.GenerateEncodedToken(user.IdentityId, user.UserName);
                    var newRefreshToken = _tokenFactory.GenerateToken();
                    user.RemoveRefreshToken(refreshToken);
                    user.AddRefreshToken(refreshToken, user.Id, "");
                    await _userReposytory.Update(user);

                    json.StatusCode = (int)HttpStatusCode.OK;
                    json.Content = JsonConvert.SerializeObject(new RefreshResponce(jwtToken, newRefreshToken, true), settings);

                    return json;

                }
            }

            json.StatusCode = (int)HttpStatusCode.BadRequest;
            json.Content = JsonConvert.SerializeObject(new RefreshResponce(null, null, false, "Invalid token"), settings);
            return json;
        }

        /// <summary>
        /// Gets user principal from his/her access token
        /// </summary>
        /// <param name="token">user's access token</param>
        /// <returns></returns>
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
