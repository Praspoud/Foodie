using Foodie.Common.Models;
using Foodie.Services.User;
using Foodie.Services.User.ViewModels;
using Foodie.Utilities;

namespace Foodie.Apis.User
{
    public static class UserPostApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            var root = "api/user/posts";
            app.MapPost(root, CreatePost);
        }

        private static IResult<int> CreatePost(IUserPostService service, IFoodieSessionAccessor accessor, UserPostVM model)
        {
            return service.CreatePost(model, accessor.UserId);
        }
    }
}
