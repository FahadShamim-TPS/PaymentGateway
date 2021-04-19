using System.Collections.Generic;
using System.Security.Claims;
using PaymentGateway_API.ModelForJWT;

namespace PaymentGateway_API.Managers
{
    public interface IAuthService
    {
        string SecretKey { get; set; }

        bool IsTokenValid(string token);
        string GenerateToken(IAuthContainerModel model);
        IEnumerable<Claim> GetTokenClaims(string token);
    }
}
