using Foodie.Common.Models;
using Foodie.Services.Post.ViewModels;

namespace Foodie.Services.Post
{
    public interface IUserPostService
    {
        IResult<int> CreatePost(UserCreatePostVM model, int UserId);
        IResult<UserPostVM> GetPostById(int postId);
        IResult<ListVM<UserPostVM>> GetUserPosts(int userId, int page, int pageSize);
        IResult<int> UpdatePost(int postId, UserCreatePostVM model, int userId);
        IResult<bool> DeletePost(int postId, int userId);
    }
}
