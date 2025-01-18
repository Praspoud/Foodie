namespace Foodie.Services.User.ViewModels
{
    public class UserPostVM
    {
        public string Content { get; set; }
        public IFormFile? MediaFile { get; set; }
        public string? MediaUrl { get; set; }
    }
}
