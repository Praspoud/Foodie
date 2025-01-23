using Foodie.Common.Models;
using Foodie.Services.Post;
using Foodie.Services.Post.ViewModels;
using Foodie.Services.Restaurant.ViewModels;
using Foodie.Services.Restaurant;
using Foodie.Utilities;
using Microsoft.AspNetCore.Mvc;
using Foodie.Models;

namespace Foodie.Apis.Posts
{
    public static class UserPostApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            var root = "api/user/post";
            app.MapPost(root, CreatePost).DisableAntiforgery();
            app.MapGet(root, GetPostById);
            app.MapGet(root + "/feed", GetUserPosts);
            app.MapPut(root + "/update", UpdatePost).DisableAntiforgery();
            app.MapDelete(root, DeletePost);
        }

        private static IResult<int> CreatePost(HttpRequest request, IUserPostService service, IFoodieSessionAccessor accessor, [FromForm] string? Content, [FromForm] List<int>? TaggedUserIds, [FromForm] List<string>? Hashtags, [FromForm] List<IFormFile>? Files)
        {
            var content = string.IsNullOrWhiteSpace(Content) ? null : Content;

            // Validate TaggedUserIds input: Ensure it's not empty before attempting to parse
            var taggedUserIds = TaggedUserIds ??
                                (request.Form.ContainsKey("TaggedUserIds") && !string.IsNullOrWhiteSpace(request.Form["TaggedUserIds"])
                                    ? request.Form["TaggedUserIds"].ToString().Split(',')
                                          .Where(x => !string.IsNullOrWhiteSpace(x) && int.TryParse(x, out _))
                                          .Select(int.Parse)
                                          .ToList()
                                    : new List<int>());

            // Validate Hashtags input: Ensure it's not empty before processing
            var hashtags = Hashtags ??
                           (request.Form.ContainsKey("Hashtags") && !string.IsNullOrWhiteSpace(request.Form["Hashtags"])
                               ? request.Form["Hashtags"].ToString().Split(',')
                                     .Where(x => !string.IsNullOrWhiteSpace(x))
                                     .ToList()
                               : new List<string>());

            // Validate Files input: Ensure files are handled correctly
            var files = Files ?? (request.Form.Files.Count > 0
                                    ? request.Form.Files.ToList()
                                    : new List<IFormFile>());
            //var content = request.Form["Content"];
            //var taggedUserIds = request.Form["TaggedUserIds"].ToString().Split(',').Select(int.Parse).ToList();
            //var hashtags = request.Form["Hashtags"].ToString().Split(',').ToList();
            //var files = request.Form.Files.ToList();

            UserCreatePostVM model = new()
            {
                Content = content,
                TaggedUserIds = taggedUserIds,
                Hashtags = hashtags,
                MediaFiles = files
            };

            return service.CreatePost(model, accessor.UserId);
        }

        private static IResult<UserPostVM> GetPostById(IUserPostService service, int postId)
        {
            return service.GetPostById(postId);
        }
        
        private static IResult<ListVM<UserPostVM>> GetUserPosts(IUserPostService service, int userId, int skip = 1, int take = 10)
        {
            return service.GetUserPosts(userId, skip, take);
        }

        private static IResult<int> UpdatePost(HttpRequest request, IUserPostService service, IFoodieSessionAccessor accessor, int postId, [FromForm] string? Content, [FromForm] List<int>? TaggedUserIds, [FromForm] List<string>? Hashtags, [FromForm] List<IFormFile>? Files)
        {
            var content = string.IsNullOrWhiteSpace(Content) ? null : Content;

            // Validate TaggedUserIds input: Ensure it's not empty before attempting to parse
            var taggedUserIds = TaggedUserIds ??
                                (request.Form.ContainsKey("TaggedUserIds") && !string.IsNullOrWhiteSpace(request.Form["TaggedUserIds"])
                                    ? request.Form["TaggedUserIds"].ToString().Split(',')
                                          .Where(x => !string.IsNullOrWhiteSpace(x) && int.TryParse(x, out _))
                                          .Select(int.Parse)
                                          .ToList()
                                    : new List<int>());

            // Validate Hashtags input: Ensure it's not empty before processing
            var hashtags = Hashtags ??
                           (request.Form.ContainsKey("Hashtags") && !string.IsNullOrWhiteSpace(request.Form["Hashtags"])
                               ? request.Form["Hashtags"].ToString().Split(',')
                                     .Where(x => !string.IsNullOrWhiteSpace(x))
                                     .ToList()
                               : new List<string>());

            // Validate Files input: Ensure files are handled correctly
            var files = Files ?? (request.Form.Files.Count > 0
                                    ? request.Form.Files.ToList()
                                    : null);
            //var content = request.Form["Content"];
            //var taggedUserIds = request.Form["TaggedUserIds"].ToString().Split(',').Select(int.Parse).ToList();
            //var hashtags = request.Form["Hashtags"].ToString().Split(',').ToList();
            //var files = request.Form.Files.ToList();

            UserCreatePostVM model = new()
            {
                Content = content,
                TaggedUserIds = taggedUserIds,
                Hashtags = hashtags,
                MediaFiles = files
            };

            return service.UpdatePost(postId, model, accessor.UserId);
        }

        private static IResult<bool> DeletePost(IUserPostService service, IFoodieSessionAccessor accessor, int postId)
        {
            return service.DeletePost(postId, accessor.UserId);
        }
    }
}
