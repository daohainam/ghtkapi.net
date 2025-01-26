using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Ghtk.Authorization
{
    public class XClientSourceAuthenticationHandlerOptions: AuthenticationSchemeOptions
    {
        public Func<string, SecurityToken, ClaimsPrincipal, bool> ClientValidator { get; set; } = (clientSource, token, principal) => false;
        public string IssuerSigningKey { get; set; } = string.Empty;
    }
}
