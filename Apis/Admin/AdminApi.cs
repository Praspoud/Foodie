using Foodie.Common;
using Foodie.Services.Admin;
using Foodie.Services.User;
using static Foodie.Services.User.ViewModels.UserVM;

namespace Foodie.Apis.Admin
{
    public static class AdminApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            app.MapPost("api/admin/", AdminLogin).AllowAnonymous();
        }

        private static IResult<string> AdminLogin(LoginVM data, IAdminService service, IConfiguration config)
        {
            return service.AdminLogin(data, config);
        }
    }
}
