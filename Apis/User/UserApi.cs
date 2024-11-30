using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using Foodie.Common;
using Foodie.Services.User;
using static Foodie.Services.User.ViewModels.UserVM;

namespace Foodie.Apis.User
{
    public static class UserApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            var root = "api/user/";
            app.MapPost(root + "login", Login).AllowAnonymous();
            //app.MapPost(root + "forgotPassword", ForgotPassword).AllowAnonymous();
            //app.MapPost(root + "verifyOtp", VerifyOtp).AllowAnonymous();
            //app.MapPost(root + "changePassword", ChangePassword);
            app.MapPost(root + "signup", SignUp).AllowAnonymous();
            //app.MapPut(root, Update);
            //app.MapPut(root + "StatusChange", UpdateUserStatus);
            //app.MapGet(root, Get);
            //app.MapGet(root + "List", List);
            //app.MapDelete(root, Delete);
        }

        private static IResult<int> SignUp(IUserService service, UserRegistrationVM data)
        {
            return service.SignUp(data);
        }

        private static IResult<string> Login(LoginVM data, IUserService service, IConfiguration config)
        {
            return service.Login(data, config);
        }
    }
}
