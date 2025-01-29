using Foodie.Apis.Admin;
using Foodie.Apis.Posts;
using Foodie.Apis.Restaurant;
using Foodie.Apis.User;

namespace Foodie
{
    public static class RegisterEndpoints
    {
        public static void RegisterApi(this WebApplication app)
        {
            UserApi.RegisterApi(app);
            AdminApi.RegisterApi(app);
            RestaurantApi.RegisterApi(app);
            UserPostApi.RegisterApi(app);
            PostReactionApi.RegisterApi(app);
            PostCommentApi.RegisterApi(app);
            FollowerApi.RegisterApi(app);
            UserBlockApi.RegisterApi(app);
        }
    }
}
