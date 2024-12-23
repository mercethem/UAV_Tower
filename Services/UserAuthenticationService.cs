using UAV_Tower.Models;
using Microsoft.EntityFrameworkCore;
using UAV_Tower.Data;

namespace UAV_Tower.Services
{
    public class UserAuthenticationService
    {
        private readonly ApplicationDbContext _context;

        public UserAuthenticationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Login(string username, string password)
        {
            var users = _context.users.FirstOrDefault(u => u.Username == username && u.Password == password);

            return users;
        }
    }
}
