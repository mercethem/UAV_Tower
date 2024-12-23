using AuthenticationServiceUser.Person;

namespace AuthenticationServiceUser.User
{
    public abstract class BaseUser : BasePerson, IUser
    {
        protected string UserID { get; private set; } // User ID property for the user.
        private string Password { get; } // User's password, should be securely stored.
        private bool isAuthenticated; // Flag to check if the user is authenticated.

        protected BaseUser(string citizenID, string name, string surname, string userID, string password)
            : base(citizenID, name, surname) // Initialize BasePerson properties. // BasePerson özelliklerini başlat.
        {
            UserID = userID;
            Password = password;
            isAuthenticated = false;
        }

        public virtual bool LogIn(string userID, string password) // Method for signing in the user. // Kullanıcıyı giriş yaptırmak için metod.
        {
            if (Password == password)
            {
                isAuthenticated = true;
                return true;
            }
            return false;
        }

        public abstract void LogOut(); // Abstract method for signing out the user. // Kullanıcıyı çıkarmak için soyut metod.

        public override string DisplayInfo() // Override to display user information. // Kullanıcı bilgisini göstermek için override et.
        {
            if (isAuthenticated)
            {
                return UserID;
            }
            else
            {
                return "User is not authenticated.";
            }
        }
    }
}
