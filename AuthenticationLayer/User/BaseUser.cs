using AuthenticationServiceUser.Person;

namespace AuthenticationServiceUser.User
{
    public abstract class BaseUser : BasePerson, IUser
    {
        protected string UserID { get; private set; } // User ID property for the user. // Kullanıcı için kullanıcı ID'si özelliği.
        private string Password { get; } // User's password, should be securely stored. // Kullanıcının şifresi, güvenli bir şekilde saklanmalıdır.
        private bool isAuthenticated; // Flag to check if the user is authenticated. // Kullanıcının doğrulanıp doğrulanmadığını kontrol etmek için bayrak.

        protected BaseUser(string citizenID, string name, string surname, string userID, string password)
            : base(citizenID, name, surname) // Initialize BasePerson properties. // BasePerson özelliklerini başlat.
        {
            UserID = userID; // Set the UserID. // Kullanıcı ID'sini ayarla.
            Password = password; // Store password securely (hashing in real scenarios). // Şifreyi güvenli bir şekilde sakla (gerçek senaryolarda hashing).
            isAuthenticated = false; // Initially, the user is not authenticated. // Başlangıçta kullanıcı doğrulanmamıştır.
        }

        public virtual bool LogIn(string userID, string password) // Method for signing in the user. // Kullanıcıyı giriş yaptırmak için metod.
        {
            if (Password == password) // Check if the provided password matches. // Verilen şifrenin eşleşip eşleşmediğini kontrol et.
            {
                isAuthenticated = true; // Set authentication flag. // Doğrulama bayrağını ayarla.
                return true; // Return true if sign-in is successful. // Giriş başarılıysa true döndür.
            }
            return false; // Failed to sign in. // Giriş başarısız oldu.
        }

        public abstract void LogOut(); // Abstract method for signing out the user. // Kullanıcıyı çıkarmak için soyut metod.

        public override string DisplayInfo() // Override to display user information. // Kullanıcı bilgisini göstermek için override et.
        {
            if (isAuthenticated) // Check if the user is authenticated. // Kullanıcının doğrulanıp doğrulanmadığını kontrol et.
            {
                return UserID; // Return the UserID if authenticated. // Doğrulanmışsa Kullanıcı ID'sini döndür.
            }
            else
            {
                return "User is not authenticated."; // Message for unauthenticated users. // Doğrulanmamış kullanıcılar için mesaj.
            }
        }
    }
}
