using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("password_hist")]
    public class EPasswordHist
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Column("password_text")]
        [MaxLength(256)]
        public string PasswordText { get; set; }

        //[Column("is_system_gen_password", TypeName = "bit")]
        //public bool IsSystemGeneratedPassword { get; set; }

        [Column("password_create_date")]
        public DateTime PasswordCreateDate { get; set; }

        [Column("is_password_active")]
        public bool IsPasswordActive { get; set; }
        public virtual EUsers Users { get; set; }
    }
}
