using AuthenticationServiceUser.User;

namespace AuthenticationServiceUser.AuthenticationService
{
    public interface IUserAuthenticationService
    {
        BaseUser Login(string userID, string password); // Method for logging in users. // Kullanıcıları giriş yaptırmak için metod.
    }
}
