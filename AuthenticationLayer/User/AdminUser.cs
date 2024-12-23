namespace AuthenticationServiceUser.User
{
    public class AdminUser : BaseUser
    {
        public AdminUser(string citizenID, string name, string surname, string userID, string password)
            : base(citizenID, name, surname, userID, password) // Initialize the base user properties.
        {
        }

        public override void LogOut() // Implement sign-out for admin users. 
        {
            Console.WriteLine($"Admin {Name} signed out.");
            
        }
    }
}
