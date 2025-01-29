using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("users")]
    public class EUsers
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_name")]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Column("password_hash")]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [Column("email")]
        [MaxLength(50)]
        public string EmailAddress { get; set; }

        [Column("first_name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Column("last_name")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Column("tran_date")]
        public DateTime TranDate { get; set; }
    }

    [Table("blocked_users")]
    public class EBlockedUsers
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("blocker_id")]
        public int BlockerId { get; set; }
        public virtual EUsers Blocker { get; set; }

        [Column("blocked_id")]
        public int BlockedId { get; set; }
        public virtual EUsers Blocked { get; set; }

        public DateTime BlockedAt { get; set; } = DateTime.UtcNow;
    }
}
