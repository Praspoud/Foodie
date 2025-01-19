using Foodie.Common.Models;
using Foodie.Services.User.ViewModels;
using static Foodie.Services.User.ViewModels.UserVM;

namespace Foodie.Services.User
{
    public interface IFollowerService
    {
        IResult<int> FollowUser(int followerId, int followeeId);
        IResult<bool> UnFollowUser(int followerId, int followeeId);
        IResult<ListVM<FollowVM>> GetFollowersList(int userId);
        IResult<ListVM<FollowVM>> GetFollowingsList(int userId);
    }
}
