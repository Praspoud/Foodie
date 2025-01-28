using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("post_reactions")]
    public class EPostReactions
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

        [Column("reaction_type")]
        public ReactionType ReactionType { get; set; }

        [Column("reacted_at")]
        public DateTime ReactedAt { get; set; }

        public virtual EUsers Users { get; set; }
        public virtual EUserPosts UserPosts { get; set; }
    }

    public enum ReactionType
    {
        Like,
        Love,
        Haha,
        Wow,
        Sad,
        Angry
    }
}
