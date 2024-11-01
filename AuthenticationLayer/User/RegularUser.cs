namespace User.User
{
    public class RegularUser : BaseUser
    {
        public RegularUser(string citizenID, string name, string surname, string userID, string password)
            : base(citizenID, name, surname, userID, password) // Initialize the base user properties. // Temel kullanıcı özelliklerini başlat.
        {
        }

        public override void LogOut() // Implement sign-out for regular users. // Normal kullanıcılar için çıkış işlemini uygula.
        {
            Console.WriteLine($"{Name} signed out."); // Output sign-out message. // Çıkış mesajını ekrana yazdır.
            // Mark user as signed out. // Kullanıcıyı çıkmış olarak işaretle.
            // Here we could set isAuthenticated to false or handle it according to application logic. // Burada isAuthenticated'ı false olarak ayarlayabiliriz veya uygulama mantığına göre ele alabiliriz.
        }
    }
}
