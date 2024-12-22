using UAV_Tower.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace UAV_Tower.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> users { get; set; }
    }
}
