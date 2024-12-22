using AuthenticationServiceUser.User;

namespace AuthenticationServiceUser.AuthenticationService
{
    public abstract class BaseUserAuthenticationService : IUserAuthenticationService
    {
        public abstract BaseUser Login(string username, string password);
    }
}
