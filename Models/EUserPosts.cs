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
        public string? Content { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<EPostMedia> Media { get; set; } = new List<EPostMedia>();
        public virtual EUsers Users { get; set; }
    }

    [Table("post_media")]
    public class EPostMedia
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("post_id")]
        [ForeignKey("UserPosts")]
        public int PostId { get; set; }

        [Column("media_url")]
        public string MediaUrl { get; set; }

        [Column("media_type")]
        public string MediaType { get; set; }

        public virtual EUserPosts UserPosts { get; set; }
    }
}