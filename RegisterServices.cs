using Foodie.Services.Admin;
using Foodie.Services.User;

namespace Foodie
{
    public static class RegisterService
    {
        public static void RegisterServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAdminService, AdminService>();
        }
    }
}
