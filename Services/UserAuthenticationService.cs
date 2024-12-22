using UAV_Tower.Models;
using Microsoft.EntityFrameworkCore;
using UAV_Tower.Data;

namespace UAV_Tower.Services
{
    public class UserAuthenticationService
    {
        private readonly ApplicationDbContext _context;

        // Constructor ile DbContext'i alıyoruz
        public UserAuthenticationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Veritabanından kullanıcıyı kontrol etme
        public User Login(string username, string password)
        {
            // Şifreyi hashleyip veritabanından karşılaştırmak için ek işlemler yapılabilir
            var users = _context.users
                               .FirstOrDefault(u => u.Username == username && u.Password == password);

            return users;
        }
    }
}
