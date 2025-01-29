using Foodie.Common.Models;
using Foodie.Services.Post;
using Foodie.Services.User;
using Foodie.Services.User.ViewModels;
using Foodie.Utilities;
using static Foodie.Services.User.ViewModels.UserVM;

namespace Foodie.Apis.User
{
    public static class UserBlockApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            var root = "api/users/";
            app.MapPost(root + "block/", BlockUser);
            app.MapDelete(root + "unblock/", UnBlockUser);
            app.MapGet(root + "blockeduserslist/", GetBlockedUsersList);
            app.MapGet(root + "isblocked/", IsBlocked);
        }

        private static IResult<bool> BlockUser(IUserBlockService service, IFoodieSessionAccessor accessor, int blockedId)
        {
            return service.BlockUser(accessor.UserId, blockedId);
        }

        private static IResult<bool> UnBlockUser(IUserBlockService service, IFoodieSessionAccessor accessor, int blockedId)
        {
            return service.UnBlockUser(accessor.UserId, blockedId);
        }

        private static IResult<ListVM<BlockedUserVM>> GetBlockedUsersList(IUserBlockService service, IFoodieSessionAccessor accessor)
        {
            return service.GetBlockedUsersList(accessor.UserId);
        }

        private static IResult<bool> IsBlocked(IUserBlockService service, IFoodieSessionAccessor accessor, int otherUserId)
        {
            return service.IsBlocked(accessor.UserId, otherUserId);
        }
    }
}
