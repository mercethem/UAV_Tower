using AuthenticationServiceUser.Person;

namespace AuthenticationServiceUser.User
{
    public interface IUser : IPerson
    {
        bool LogIn(string UserID, string password); // Method for user sign-in.
        void LogOut(); // Method for user sign-out.
    }
}
