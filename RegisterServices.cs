using Foodie.Common.Services;
using Foodie.Services.Admin;
using Foodie.Services.Post;
using Foodie.Services.Restaurant;
using Foodie.Services.User;

namespace Foodie
{
    public static class RegisterService
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAdminService, AdminService>();
            builder.Services.AddTransient<IRestaurantService, RestaurantService>();
            builder.Services.AddTransient<IUserPostService, UserPostService>();
            builder.Services.AddTransient<IPostReactionService, PostReactionService>();
            builder.Services.AddTransient<IPostCommentService, PostCommentService>();
            builder.Services.AddTransient<IFollowerService, FollowerService>();
            builder.Services.AddSingleton<FileUpload>();
        }
    }
}
