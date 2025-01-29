using Foodie.Common.Models;
using Foodie.Services.Post;
using Foodie.Services.User;
using Foodie.Utilities;
using static Foodie.Services.User.ViewModels.UserVM;

namespace Foodie.Apis.User
{
    public static class FollowerApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            var root = "api/users/";
            app.MapPost(root + "follow/", FollowUser);
            app.MapDelete(root + "unfollow/", UnFollowUser);
            app.MapGet(root + "followerslist/", GetFollowersList);
            app.MapGet(root + "followingslist/", GetFollowingsList);
            app.MapGet(root + "mutualfollowerslist/", GetMutualFollowers);
        }

        private static IResult<int> FollowUser(IFollowerService service, IFoodieSessionAccessor accessor, int followeeId)
        {
            return service.FollowUser(accessor.UserId, followeeId);
        }

        private static IResult<bool> UnFollowUser(IFollowerService service, IFoodieSessionAccessor accessor, int followeeId)
        {
            return service.UnFollowUser(accessor.UserId, followeeId);
        }
        private static IResult<ListVM<FollowVM>> GetFollowersList(IFollowerService service, IFoodieSessionAccessor accessor)
        {
            return service.GetFollowersList(accessor.UserId);
        }

        private static IResult<ListVM<FollowVM>> GetFollowingsList(IFollowerService service, IFoodieSessionAccessor accessor)
        {
            return service.GetFollowingsList(accessor.UserId);
        }

        private static IResult<ListVM<FollowVM>> GetMutualFollowers(IFollowerService service, IFoodieSessionAccessor accessor, int otherUserId)
        {
            return service.GetMutualFollowers(accessor.UserId, otherUserId);
        }
    }
}
