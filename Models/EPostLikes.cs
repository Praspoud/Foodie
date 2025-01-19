using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("post_likes")]
    public class EPostLikes
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_id")]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Column("post_id")]
        [ForeignKey("UserPosts")]
        public int PostId { get; set; }

        [Column("liked_at")]
        public DateTime LikedAt { get; set; }

        public virtual EUsers Users { get; set; }
        public virtual EUserPosts UserPosts { get; set; }


    }
}
