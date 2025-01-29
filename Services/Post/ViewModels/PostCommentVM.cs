namespace Foodie.Services.Post.ViewModels
{
    public class PostCommentVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public List<MentionedUserVM>? MentionedUsers { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class MentionedUserVM
    {
        public int MentionedUserId { get; set; }
        public string MentionedUserName { get; set; }
    }
}
