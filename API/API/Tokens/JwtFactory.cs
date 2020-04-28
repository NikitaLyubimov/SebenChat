using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

using API.ViewModels;

namespace API.Tokens
{
    public class JwtFactory
    {
        private JwtSecurityOptions _securityOptions;
        public JwtFactory(JwtSecurityOptions options)
        {
            _securityOptions = options;
        }

        public async Task<AccessToken> GenerateEncodedToken(string id, string userName)
        {
            var identity = GenerateClaimsIdentity(id, userName);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _securityOptions.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, await _securityOptions.JTIGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_securityOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst("rol"),
                identity.FindFirst("id")
            };

            var jwtToken = new JwtSecurityToken(_securityOptions.Issuer, _securityOptions.Audience, 
                claims, _securityOptions.NotBefore, _securityOptions.ExpDate, _securityOptions.SigningCredentials);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            return new AccessToken(tokenHandler.WriteToken(jwtToken), (int)_securityOptions.ValidFor.TotalSeconds);
        }

        private long ToUnixEpochDate(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);


        private ClaimsIdentity GenerateClaimsIdentity(string id, string userName)
        {
            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
            {
                new Claim("id", id),
                new Claim("rol", "api_access")
            });

        }
    }
}
