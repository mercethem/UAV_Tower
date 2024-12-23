using AuthenticationServiceUser.User;

namespace AuthenticationServiceUser.AuthenticationService
{
    public class UserAuthenticationService : BaseUserAuthenticationService
    {
        public override BaseUser Login(string username, string password) // Method to authenticate user.
        {
            if (username == "admin" && password == "adminpass")  
            {
                var adminUser = new AdminUser("12345", "Admin", "User", "admin001", "adminpass");
                if (adminUser.LogIn(username, password)) 
                {
                    return adminUser; 
                }
            }
            else if (username == "user" && password == "userpass")
            {
                var regularUser = new RegularUser("67890", "Regular", "User", "user001", "userpass"); 
                if (regularUser.LogIn(username, password))
                {
                    return regularUser;
                }
            }

            Console.WriteLine("Login failed: Invalid credentials."); 
            return null;
        }
    }
}
