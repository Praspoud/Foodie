using Foodie.Common.Models;
using Foodie.Services.User.ViewModels;

namespace Foodie.Services.User
{
    public interface IUserPostService
    {
        IResult<int> CreatePost(UserPostVM model, int UserId);
    }
}
