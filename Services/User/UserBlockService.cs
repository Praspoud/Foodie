using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.User.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Foodie.Services.User.ViewModels.UserVM;

namespace Foodie.Services.User
{
    public class UserBlockService : IUserBlockService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<EBlockedUsers> _blockedUserService;

        public UserBlockService(IServiceFactory factory)
        {
            _factory = factory;
            _blockedUserService = _factory.GetInstance<EBlockedUsers>();
        }
        public IResult<bool> BlockUser(int blockerId, int blockedId)
        {
            try
            {
                var alreadyBlocked = _blockedUserService.List().FirstOrDefault(b => b.BlockerId == blockerId && b.BlockedId == blockedId);
                if (blockerId == blockedId || alreadyBlocked != null)
                {
                    return new IResult<bool>
                    {
                        Data = false,
                        Message = "Invalid Block.",
                        Status = ResultStatus.Failure
                    };
                }

                var block = new EBlockedUsers
                {
                    BlockerId = blockerId,
                    BlockedId = blockedId
                };

                _blockedUserService.Add(block);

                return new IResult<bool>
                {
                    Data = true,
                    Message = "Blocked successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<bool>
                {
                    Data = false,
                    Status = ResultStatus.Failure,
                    Message = "Block Failed."
                };
            }
        }

        public IResult<bool> UnBlockUser(int blockerId, int blockedId)
        {
            try
            {
                var block = _blockedUserService.List().FirstOrDefault(f => f.BlockerId == blockerId && f.BlockedId == blockedId);
                if (block == null)
                {
                    return new IResult<bool>
                    {
                        Data = false,
                        Message = "UnBlock Failed.",
                        Status = ResultStatus.Failure
                    };
                }
                _blockedUserService.Remove(block);

                return new IResult<bool>
                {
                    Data = true,
                    Message = "UnBlocked Successfully.",
                    Status = ResultStatus.Failure
                };
            }
            catch (Exception ex)
            {
                return new IResult<bool>
                {
                    Message = "UnBlock Failed.",
                    Status = ResultStatus.Failure
                };
            }
        }

        public IResult<ListVM<BlockedUserVM>> GetBlockedUsersList(int userId)
        {
            try
            {
                var blockedUsers = _blockedUserService.List().Where(f => f.BlockerId == userId);
                var blockedUserList = blockedUsers.Select(f => new BlockedUserVM
                {
                    Id = f.Blocked.Id,
                    UserName = f.Blocked.UserName,
                    FirstName = f.Blocked.FirstName,
                    LastName = f.Blocked.LastName,
                }).ToList();

                return new IResult<ListVM<BlockedUserVM>>
                {
                    Data = new ListVM<BlockedUserVM>
                    {
                        List = blockedUserList,
                        Count = blockedUsers.Count()
                    },
                    Message = "Blocked Users retrieved successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<ListVM<BlockedUserVM>>
                {
                    Status = ResultStatus.Failure,
                    Message = "Failed to retrieve blocked users."
                };
            }
        }

        public IResult<bool> IsBlocked(int userId, int otherUserId)
        {
            var isblocked =  _blockedUserService.List()
                .Any(b => (b.BlockerId == userId && b.BlockedId == otherUserId) ||
                          (b.BlockerId == otherUserId && b.BlockedId == userId));
            if (isblocked == true)
            {
                return new IResult<bool>
                {
                    Data = isblocked,
                    Message = "Blocked.",
                    Status = ResultStatus.Success
                };
            }
            else
            {
                return new IResult<bool>
                {
                    Data = isblocked,
                    Message = "Not Blocked.",
                    Status = ResultStatus.Failure
                };
            }
        }
    }
}
