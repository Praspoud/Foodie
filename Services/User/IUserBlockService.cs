using Foodie.Common.Models;
using Foodie.Services.User.ViewModels;

namespace Foodie.Services.User
{
    public interface IUserBlockService
    {
        IResult<bool> BlockUser(int blockerId, int blockedId);
        IResult<bool> UnBlockUser(int blockerId, int blockedId);
        IResult<ListVM<BlockedUserVM>> GetBlockedUsersList(int userId);
        IResult<bool> IsBlocked(int userId, int otherUserId);
    }
}
