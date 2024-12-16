using Foodie.Common;
using System.Collections.Generic;
using static Foodie.Services.User.ViewModels.UserVM;

namespace Foodie.Services.User
{
    public interface IUserService
    {
        IResult<string> Login(LoginVM data, IConfiguration config);
        //IResult<string> ForgotPassword(ForgotPasswordVM data);
        //IResult<string> VerifyOtp(VerifyOtpVM data);
        IResult<int> SignUp(UserRegistrationVM data);
        IResult<int> Update(UserUpdateVM data, int userId);
        //IResult<int> UpdateUserStatus(int StatusChangeId, int userId);
        IResult<UserUpdateVM> Get(int userId);
        //IResult<string> ChangePassword(ChangePasswordVM data, int userId, IConfiguration config);
        //IResult<ListVM<UserRegistrationVM>> List(string name, OrderBy orderBy, int skip, int take);
        //IResult<bool> Delete(int id, int userId);
    }
}
