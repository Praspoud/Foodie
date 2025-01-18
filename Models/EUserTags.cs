using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("user_tags")]
    public class EUserTags
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("post_id")]
        [ForeignKey("UserPosts")]
        public int PostId { get; set; }

        [Column("tagged_user_id")]
        [ForeignKey("Users")]
        public int TaggedUserId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        public virtual EUserPosts UserPosts { get; set; }
        public virtual EUsers Users { get; set; }
    }
}
