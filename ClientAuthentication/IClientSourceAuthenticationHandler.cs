namespace ClientAuthentication
{
    public interface IClientSourceAuthenticationHandler
    {
        bool Validate(string clientSource);
    }
}
