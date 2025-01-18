using Foodie.Common.Models;
using Foodie.Services.User;
using Foodie.Services.User.ViewModels;
using Foodie.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Foodie.Apis.User
{
    public static class UserPostApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            var root = "api/user/posts";
            app.MapPost(root, CreatePost).DisableAntiforgery(); ;
        }

        private static IResult<int> CreatePost(HttpRequest request, IUserPostService service, IFoodieSessionAccessor accessor, [FromForm] string Content, [FromForm] List<int> TaggedUserIds, [FromForm] List<string> Hashtags)
        {
            //UserPostVM model = new();
            //model.MediaFile = (request.Form.Files.Count > 0) ? request.Form.Files[0] : null;
            //model.Content = Content;
            //model.TaggedUserIds = TaggedUserIds;
            //model.Hashtags = Hashtags;
            //return service.CreatePost(model, accessor.UserId);

            var content = request.Form["Content"];
            var taggedUserIds = request.Form["TaggedUserIds"].ToString().Split(',').Select(int.Parse).ToList(); // Convert to List<int>
            var hashtags = request.Form["Hashtags"].ToString().Split(',').ToList(); // Convert to List<string>

            UserPostVM model = new()
            {
                Content = content,
                TaggedUserIds = taggedUserIds,
                Hashtags = hashtags,
                MediaFile = (request.Form.Files.Count > 0) ? request.Form.Files[0] : null
            };

            return service.CreatePost(model, accessor.UserId);
        }
    }
}
