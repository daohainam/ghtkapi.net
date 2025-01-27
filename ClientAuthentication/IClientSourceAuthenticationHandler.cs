namespace ClientAuthentication
{
    public interface IClientSourceAuthenticationHandler
    {
        Task<bool> ValidateAsync(string clientSource);
    }
}
