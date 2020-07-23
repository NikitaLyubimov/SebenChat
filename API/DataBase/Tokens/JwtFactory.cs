using System;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;

using Core.Interfaces.Services;
using Core.DTO;


namespace Infrustructure.Tokens
{
    internal sealed class JwtFactory : IJwtFactory
    {
        private JwtSecurityOptions _securityOptions;

        public JwtFactory(JwtSecurityOptions securityOptions)
        {
            _securityOptions = securityOptions;
        }
        public async Task<AccessToken> GenerateEncodedToken(string id, string userName)
        {
            var identity = GenerateClaimsIdentity(id, userName);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await _securityOptions.JTIGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_securityOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst("rol"),
                identity.FindFirst("id")
            };

            var jwtToken = new JwtSecurityToken(_securityOptions.Issuer, _securityOptions.Audience, claims,
                _securityOptions.NotBefore, _securityOptions.ExpDate, _securityOptions.SigningCredentials);

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
