using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using DataBase.Repositories;
using DataBase.DTO;
using DataBase.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using System.Web;
using System.Text.RegularExpressions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyController : ControllerBase
    {
        private EmailTokenReposytory _tokenRep;
        private UserManager<AppUser> _uManager;
        private UserReposytory _userRep;
        public VerifyController(EmailTokenReposytory tokenRep, UserManager<AppUser> uManager, UserReposytory userRep)
        {
            _tokenRep = tokenRep;
            _uManager = uManager;
            _userRep = userRep;
        }
        public async Task<ActionResult> Index([FromQuery]string token)
        {
            var decToken = HttpUtility.UrlDecode(token);
            var newToken = Regex.Replace(decToken, "%2b", "+", RegexOptions.IgnoreCase);

            var confToken = await _tokenRep.GetToken(newToken);
            ContentResult json = new ContentResult();
            json.ContentType = "application/json";
            JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), NullValueHandling = NullValueHandling.Ignore };

            if(confToken == null)
            {
                json.Content = JsonConvert.SerializeObject(new Error("error", "Error"), settings);
                return json;
            }
            else
            {
                var user = await _userRep.GetById(confToken.UserId);

                var appUser = await _uManager.FindByIdAsync(user.IdentityId);
                appUser.EmailConfirmed = true;
                await _uManager.UpdateAsync(appUser);

                await _tokenRep.Delete(confToken);


                json.Content = JsonConvert.SerializeObject("success", settings);
                return json;
            }


        }
    }
}