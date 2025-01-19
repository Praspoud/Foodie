using Foodie.Common.Models;

namespace Foodie.Services.Post
{
    public interface IPostLikeService
    {
        IResult<int> LikePost(int userId, int postId);
        IResult<int> UnLikePost(int userId, int postId);
        IResult<int> GetLikeCount(int postId);
        IResult<bool> IsPostLikedByUser(int userId, int postId);
    }
}
