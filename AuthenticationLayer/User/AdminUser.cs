namespace User.User
{
    public class AdminUser : BaseUser
    {
        public AdminUser(string citizenID, string name, string surname, string userID, string password)
            : base(citizenID, name, surname, userID, password) // Initialize the base user properties. // Temel kullanıcı özelliklerini başlat.
        {
        }

        public override void LogOut() // Implement sign-out for admin users. // Yönetici kullanıcılar için çıkış işlemini uygula.
        {
            Console.WriteLine($"Admin {Name} signed out."); // Output sign-out message. // Çıkış mesajını ekrana yazdır.
            // Similar handling as in RegularUser. // Normal kullanıcıda olduğu gibi benzer işlem.
        }
    }
}
