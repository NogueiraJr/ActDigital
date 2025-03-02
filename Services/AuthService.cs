public class AuthService : IAuthService
{
    // Este é apenas um exemplo simplificado. Em um código real, seria usado um banco de dados seguro.
    private readonly Dictionary<string, string> _users = new Dictionary<string, string>
    {
        { "test", "password" }
    };

    public bool ValidateUser(string username, string password)
    {
        return _users.TryGetValue(username, out var storedPassword) && storedPassword == password;
    }
}