using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Ghtk.Authorization
{
    public class XClientSourceAuthenticationHandler : AuthenticationHandler<XClientSourceAuthenticationHandlerOptions>
    {
        public XClientSourceAuthenticationHandler(IOptionsMonitor<XClientSourceAuthenticationHandlerOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var clientSource = Context.Request.Headers["X-Client-Source"];
            var token = Context.Request.Headers["Token"];

            if (clientSource.Count == 0)
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing X-Client-Source header"));
            }

            if (token.Count == 0)
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Token header"));
            }


            var clientSourceValue = clientSource.FirstOrDefault();
            var tokenValue = token.FirstOrDefault();

            if (!string.IsNullOrEmpty(clientSourceValue) &&
                !string.IsNullOrEmpty(tokenValue) &&
                VerifyClient(clientSourceValue, tokenValue, out var principal))
            {
                //var identity = new ClaimsIdentity(Scheme.Name);
                //identity.AddClaim(new Claim(ClaimTypes.Name, clientSourceValue));
                //var principal = new ClaimsPrincipal(identity);
                ((ClaimsIdentity)principal!.Identity!).AddClaim(new Claim("PartnerId", clientSourceValue));
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            else
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));
            }
        }

        private bool VerifyClient(string clientSourceValue, string tokenValue, out ClaimsPrincipal? principal)
        {
            if (!Validate(tokenValue, out var token, out principal))
            {
                return false;
            }

            var sub = (token as JwtSecurityToken)!.Subject;

            if (clientSourceValue != sub)
            {
                return false;
            }

            if (!Options.ClientValidator(clientSourceValue, token!, principal!))
            {
                return false;
            }

            return true;
        }

        private bool Validate(string tokenValue, out SecurityToken? token, out ClaimsPrincipal? claimsPrincipal)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Options.IssuerSigningKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                claimsPrincipal = handler.ValidateToken(tokenValue, tokenValidationParameters, out token);

                return true;
            }
            catch (Exception)
            {
                token = null;
                claimsPrincipal = null;

                return false;
            }
        }
    }
}
