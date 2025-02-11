namespace API.v1.Services
{
    public interface IAuthService
    {
        Task<string?> Authenticate(string username, string password);
    }
}