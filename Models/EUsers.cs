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
}
