using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AuthenApp.Infrastructure.Services
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims);
        List<Claim> GetClaims(IdentityUser user, IList<string> roles);
    }
}
