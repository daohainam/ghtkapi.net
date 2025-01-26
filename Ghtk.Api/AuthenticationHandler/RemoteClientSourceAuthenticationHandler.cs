using ClientAuthentication;

namespace Ghtk.Api.AuthenticationHandler
{
    public class RemoteClientSourceAuthenticationHandler : IClientSourceAuthenticationHandler
    {
        private static readonly HttpClient httpClient = new();
        private readonly string authenticationServiceUrl;
        public RemoteClientSourceAuthenticationHandler(string authenticationServiceUrl)
        {
            this.authenticationServiceUrl = authenticationServiceUrl;
        }

        public bool Validate(string clientSource)
        {
            // Call the remote authentication service to validate the client source

            if (string.IsNullOrEmpty(clientSource))
            {
                return false;
            }

            var response = httpClient.GetAsync($"{authenticationServiceUrl}/api/clientsource/{clientSource}").Result;

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
