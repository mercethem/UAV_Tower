using AuthenticationServiceUser.User;

namespace AuthenticationServiceUser.AuthenticationService
{
    public class UserAuthenticationService : BaseUserAuthenticationService
    {
        public override BaseUser Login(string username, string password) // Method to authenticate user. // Kullanıcıyı doğrulamak için metod.
        {
            if (username == "admin" && password == "adminpass") // Check admin credentials. // Yönetici kimlik bilgilerini kontrol et.
            {
                var adminUser = new AdminUser("12345", "Admin", "User", "admin001", "adminpass"); // Create new AdminUser. // Yeni bir AdminUser oluştur.
                if (adminUser.LogIn(username, password)) // Attempt to sign in the admin user. // Yönetici kullanıcının giriş yapmasını dene.
                {
                    return adminUser; // Return the authenticated admin user. // Doğrulanan yönetici kullanıcısını döndür.
                }
            }
            else if (username == "user" && password == "userpass") // Check regular user credentials. // Normal kullanıcı kimlik bilgilerini kontrol et.
            {
                var regularUser = new RegularUser("67890", "Regular", "User", "user001", "userpass"); // Create new RegularUser. // Yeni bir RegularUser oluştur.
                if (regularUser.LogIn(username, password)) // Attempt to sign in the regular user. // Normal kullanıcının giriş yapmasını dene.
                {
                    return regularUser; // Return the authenticated regular user. // Doğrulanan normal kullanıcısını döndür.
                }
            }

            Console.WriteLine("Login failed: Invalid credentials."); // Output login failure message. // Giriş başarısız mesajını ekrana yazdır.
            return null; // Return null if login failed. // Giriş başarısız olduysa null döndür.
        }
    }
}
