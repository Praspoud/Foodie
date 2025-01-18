using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("user_posts")]
    public class EUserPosts
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Column("content")]
        [MaxLength(255)]
        public string Content { get; set; }

        [Column("image_url")]
        public string? MediaUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual EUsers Users { get; set; }
    }
}
