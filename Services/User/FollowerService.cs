using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.Restaurant.ViewModels;
using Foodie.Services.User.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Foodie.Services.User.ViewModels.UserVM;

namespace Foodie.Services.User
{
    public class FollowerService : IFollowerService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<EFollowers> _followersService;
        private readonly IServiceRepository<EBlockedUsers> _blockedUserService;

        public FollowerService(IServiceFactory factory)
        {
            _factory = factory;
            _followersService = _factory.GetInstance<EFollowers>();
            _blockedUserService = _factory.GetInstance<EBlockedUsers>();
        }

        public IResult<int> FollowUser(int followerId, int followeeId)
        {
            try
            {
                var duplicateFollow = _followersService.List().FirstOrDefault(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
                var isblocked = _blockedUserService.List()
                    .Any(b => (b.BlockerId == followerId && b.BlockedId == followeeId) ||
                              (b.BlockerId == followeeId && b.BlockedId == followerId));
                if (followerId == followeeId)
                {
                    return new IResult<int>
                    {
                        Message = "Cannot follow oneself.",
                        Status = ResultStatus.Failure
                    };
                }

                if (duplicateFollow != null)
                {
                    return new IResult<int>
                    {
                        Message = "Already Followed.",
                        Status = ResultStatus.Failure
                    };
                }

                if (isblocked != null)
                {
                    return new IResult<int>
                    {
                        Message = "One of the user is blocked.",
                        Status = ResultStatus.Failure
                    };
                }

                var follower = new EFollowers
                {
                    FollowerId = followerId,
                    FolloweeId = followeeId,
                    FollowedAt = DateTime.UtcNow
                };

                var result = _followersService.Add(follower);

                return new IResult<int>
                {
                    Data = result.Id,
                    Message = "Followed successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<int>
                {
                    Status = ResultStatus.Failure,
                    Message = "Follow Failed."
                };
            }
        }

        public IResult<bool> UnFollowUser(int followerId, int followeeId)
        {
            try
            {
                var follower = _followersService.List().FirstOrDefault(f => f.FollowerId == followerId && f.FolloweeId == followeeId);
                if (follower == null)
                {
                    return new IResult<bool>
                    {
                        Data = false,
                        Message = "Unfollow Failed.",
                        Status = ResultStatus.Failure
                    };
                }
                _followersService.Remove(follower);

                return new IResult<bool>
                {
                    Data = true,
                    Message = "Unfollowed Successfully.",
                    Status = ResultStatus.Failure
                };
            }
            catch(Exception ex)
            {
                return new IResult<bool>
                {
                    Message = "Unfollow Failed.",
                    Status = ResultStatus.Failure
                };
            }
        }

        public IResult<ListVM<FollowVM>> GetFollowersList(int userId)
        {
            try
            {
                var followers = _followersService.List().Where(f => f.FolloweeId == userId);
                var followersList = followers.Select(f => new FollowVM
                {
                    Id = f.FollowerUsers.Id,
                    UserName = f.FollowerUsers.UserName,
                    FirstName = f.FollowerUsers.FirstName,
                    LastName = f.FollowerUsers.LastName,
                }).ToList();

                return new IResult<ListVM<FollowVM>>
                {
                    Data = new ListVM<FollowVM>
                    {
                        List = followersList,
                        Count = followers.Count()
                    },
                    Message = "Followers retrieved successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch(Exception ex)
            {
                return new IResult<ListVM<FollowVM>>
                {
                    Status = ResultStatus.Failure,
                    Message = "Failed to retrieve followers."
                };
            }
        }

        public IResult<ListVM<FollowVM>> GetFollowingsList(int userId)
        {
            try
            {
                var followings = _followersService.List().Where(f => f.FollowerId == userId);
                var followingsList = followings.Select(f => new FollowVM
                {
                    Id = f.FolloweeUsers.Id,
                    UserName = f.FolloweeUsers.UserName,
                    FirstName = f.FolloweeUsers.FirstName,
                    LastName = f.FolloweeUsers.LastName,
                }).ToList();

                return new IResult<ListVM<FollowVM>>
                {
                    Data = new ListVM<FollowVM>
                    {
                        List = followingsList,
                        Count = followings.Count()
                    },
                    Message = "Followings retrieved successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<ListVM<FollowVM>>
                {
                    Status = ResultStatus.Failure,
                    Message = "Failed to retrieve followings."
                };
            }
        }

        public IResult<ListVM<FollowVM>> GetMutualFollowers(int userId, int otherUserId)
        {
            try
            {
                var userFollowers = _followersService.List()
                    .Where(f => f.FolloweeId == userId)
                    .Select(f => new FollowVM
                    {
                        Id = f.FollowerUsers.Id,
                        UserName = f.FollowerUsers.UserName,
                        FirstName = f.FollowerUsers.FirstName,
                        LastName = f.FollowerUsers.LastName,
                    })
                    .ToList();

                var otherUserFollowers = _followersService.List()
                    .Where(f => f.FolloweeId == otherUserId)
                    .Select(f => new FollowVM
                    {
                        Id = f.FollowerUsers.Id,
                        UserName = f.FollowerUsers.UserName,
                        FirstName = f.FollowerUsers.FirstName,
                        LastName = f.FollowerUsers.LastName,
                    })
                    .ToList();

                //var mutual = userFollowers.Intersect(otherUserFollowers).ToList();
                var mutual = userFollowers
                    .Where(uf => otherUserFollowers.Any(of => of.Id == uf.Id))
                    .ToList();

                return new IResult<ListVM<FollowVM>>
                {
                    Data = new ListVM<FollowVM>
                    {
                        List = mutual,
                        Count = mutual.Count()
                    },
                    Message = "Mutual Followers retrieved successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<ListVM<FollowVM>>
                {
                    Message = "Mutual Followers retrieve failed.",
                    Status = ResultStatus.Failure
                };
            }
        }
    }
}
