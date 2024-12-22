using System.ComponentModel.DataAnnotations.Schema;

namespace UAV_Tower.Models
{
    [Table("users")] // Tablo adını doğru eşle
    public class User
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("username")] // SQL'deki username sütununa eşle
        public string Username { get; set; }

        [Column("password")] // SQL'deki password sütununa eşle
        public string Password { get; set; }
    }
}
