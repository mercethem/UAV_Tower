using System.ComponentModel.DataAnnotations.Schema;

namespace UAV_Tower.Models
{
    [Table("users")]
    public class User
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("username")] 
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }
    }
}
