﻿using Foodie.Apis.Admin;
using Foodie.Apis.User;

namespace Foodie
{
    public static class RegisterEndpoints
    {
        public static void RegisterApi(this WebApplication app)
        {
            UserApi.RegisterApi(app);
            AdminApi.RegisterApi(app);
        }
    }
}
