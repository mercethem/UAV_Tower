namespace AuthenticationServiceUser.User
{
    public class RegularUser : BaseUser
    {
        public RegularUser(string citizenID, string name, string surname, string userID, string password)
            : base(citizenID, name, surname, userID, password) // Initialize the base user properties. 
        {
        }

        public override void LogOut() // Implement sign-out for regular users.
        {
            Console.WriteLine($"{Name} signed out.");
        }
    }
}
