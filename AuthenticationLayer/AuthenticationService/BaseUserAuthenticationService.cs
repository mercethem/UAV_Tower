using User.User;

namespace User.AuthenticationService
{
    public abstract class BaseUserAuthenticationService : IUserAuthenticationService
    {
        public abstract BaseUser Login(string username, string password);
    }
}
