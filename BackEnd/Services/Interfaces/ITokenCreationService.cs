using IdentityServer4.Models;
using System.Security.Claims;

namespace BackEnd.Services.Interfaces
{
    public interface ITokenCreationService
    {
        Task<string> CreateTokenAsync(Token token);
        Token CreateAccessTokenAsync(IList<Claim> Claims);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string? token);
    }
}
