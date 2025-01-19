using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.Restaurant.ViewModels;
using Foodie.Services.User.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Foodie.Services.Post
{
    public class PostLikeService : IPostLikeService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<EPostLikes> _postLikeService;

        public PostLikeService(IHttpContextAccessor httpContextAccessor, IServiceFactory factory, IConfiguration congf, FileUpload fileUpload)
        {
            _factory = factory;
            _postLikeService = _factory.GetInstance<EPostLikes>();
        }

        public IResult<int> LikePost(int userId, int postId)
        {
            try
            {
                var isLiked = _postLikeService.FindByName(l => l.UserId == userId && l.PostId == postId);
                if (isLiked != null)
                {
                    return new IResult<int>
                    {
                        Message = "Post Already Liked.",
                        Status = ResultStatus.Success
                    };
                }
                var like = new EPostLikes
                {
                    UserId = userId,
                    PostId = postId,
                    LikedAt = DateTime.UtcNow
                };

                var result = _postLikeService.Add(like);

                return new IResult<int>
                {
                    Data = result.Id,
                    Message = "Post Liked successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<int>
                {
                    Message = "Post Like Failed.",
                    Status = ResultStatus.Success
                };
            }
        }

        public IResult<int> UnLikePost(int userId, int postId)
        {
            try
            {
                var isLiked = _postLikeService.FindByName(l => l.UserId == userId && l.PostId == postId);
                if (isLiked == null)
                {
                    return new IResult<int>
                    {
                        Message = "Post Already UnLiked.",
                        Status = ResultStatus.Success
                    };
                }
                var result = _postLikeService.Remove(isLiked);

                return new IResult<int>
                {
                    Message = "Post UnLiked successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<int>
                {
                    Message = "Post Like Failed.",
                    Status = ResultStatus.Success
                };
            }
        }

        public IResult<int> GetLikeCount(int postId)
        {
            var likes = _postLikeService.List().Count(l => l.PostId == postId);

            return new IResult<int>
            {
                Data = likes,
                Message = "Likes retrieved successfully.",
                Status = ResultStatus.Success
            };
        }

        public IResult<bool> IsPostLikedByUser(int userId, int postId)
        {
            var likes = _postLikeService.List().Any(l => l.UserId == userId && l.PostId == postId);

            return new IResult<bool>
            {
                Data = likes,
                Message = "Likes retrieved successfully.",
                Status = ResultStatus.Success
            };
        }
    }
}
