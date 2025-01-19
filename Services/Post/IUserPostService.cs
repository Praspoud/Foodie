using Foodie.Common.Models;
using Foodie.Services.Post.ViewModels;

namespace Foodie.Services.Post
{
    public interface IUserPostService
    {
        IResult<int> CreatePost(UserPostVM model, int UserId);
    }
}
