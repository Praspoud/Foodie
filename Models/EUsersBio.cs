using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("users_bio")]
    public class EUsersBio
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [ForeignKey("Users")]
        public int? UserId { get; set; }

        [Column("bio")]
        [MaxLength(1500)]
        public string? Bio { get; set; }

        public virtual EUsers Users { get; set; }
    }
}
