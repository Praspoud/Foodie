using Foodie.Common.Models;
using static Foodie.Services.User.ViewModels.UserVM;

namespace Foodie.Services.Admin
{
    public interface IAdminService
    {
        IResult<string> AdminLogin(LoginVM data, IConfiguration config);
    }
}
