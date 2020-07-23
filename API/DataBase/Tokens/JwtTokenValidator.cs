using System;
using System.Text;
using System.Security.Claims;

using Core.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Infrustructure.Tokens
{
    internal sealed class JwtTokenValidator : IJwtTokenValidator
    {
        public ClaimsPrincipal GetPrincipalsFromToken(string token, string signingKey)
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            var principal = jwtHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                ValidateLifetime = false
            }, out var securityToken);

            if (!(securityToken is JwtSecurityToken jwtSecurityToken) || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token");

            return principal;
        }
    }
}
