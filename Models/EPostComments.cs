using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Foodie.Models
{
    [Table("post_comments")]
    public class EPostComments
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

        [Column("content")]
        [MaxLength(255)]
        public string Content { get; set; }

        [Column("parent_comment_id")]
        public int? ParentCommentId { get; set; }

        [Column("is_reported")]
        public bool IsReported { get; set; } = false;

        [Column("moderation_status")]
        public string? ModerationStatus { get; set; } = "Pending"; // Pending, Approved, Rejected

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        //public virtual List<EPostComments> Replies { get; set; } = new();

        public virtual EUsers Users { get; set; }
        public virtual EUserPosts UserPosts { get; set; }

    }

    [Table("user_mentions")]
    public class EUserMentions
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("comment_id")]
        [ForeignKey("PostComments")]
        public int CommentId { get; set; }

        [Column("mentioned_user_id")]
        [ForeignKey("MentionedUsers")]
        public int MentionedUserId { get; set; }

        public virtual EPostComments PostComments { get; set; }
        public virtual EUsers MentionedUsers { get; set; }
    }
}
