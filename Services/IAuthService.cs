public interface IAuthService
{
    bool ValidateUser(string username, string password);
}