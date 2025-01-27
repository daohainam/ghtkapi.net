namespace ClientAuthentication;
public class RemoteClientSourceAuthenticationHandler(string authenticationServiceUrl) : IClientSourceAuthenticationHandler
{
    private static readonly HttpClient httpClient = new();
    private readonly string authenticationServiceUrl = authenticationServiceUrl;

    public async Task<bool> ValidateAsync(string clientSource)
    {
        // Call the remote authentication service to validate the client source

        if (string.IsNullOrEmpty(clientSource))
        {
            return false;
        }

        var response = await httpClient.GetAsync($"{authenticationServiceUrl}/api/clientsource/{clientSource}");

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        return false;
    }
}
