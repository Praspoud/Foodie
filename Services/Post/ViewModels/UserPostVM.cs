namespace Foodie.Services.Post.ViewModels
{
    public class UserCreatePostVM
    {
        public string? Content { get; set; }
        public List<int>? TaggedUserIds { get; set; }
        public List<string>? Hashtags { get; set; }
        public List<IFormFile>? MediaFiles { get; set; } = new();
        public int? RestaurantRating { get; set; }
        public string? RestaurantRatingType { get; set; }
        public string? RestaurantId { get; set; }
    }

    public class UserPostVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<string> MediaUrls { get; set; } = new();
        public List<TaggedUserVM>? TaggedUsers { get; set; }
        public List<HashTagVM>? Hashtags { get; set; }
        public int? RestaurantRating { get; set; }
        public string? RestaurantRatingType { get; set; }
        public string? RestaurantId { get; set; }
        public int ReactionsCount { get; set; }
        public int CommentsCount { get; set; }
    }

    public class TaggedUserVM
    {
        public int TaggedUserId { get; set; }
        public string TaggedUserName { get; set; }
    }

    public class HashTagVM
    {
        public int HashTagId { get; set; }
        public string HashTag { get; set; }
    }
}
