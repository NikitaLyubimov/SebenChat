using System.Security.Claims;

namespace Core.Interfaces.Services
{
    public interface IJwtTokenValidator
    {
        ClaimsPrincipal GetPrincipalsFromToken(string token);
    }
}
