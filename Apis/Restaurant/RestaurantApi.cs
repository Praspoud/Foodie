using Foodie.Common.Models;
using Foodie.Services.Restaurant;
using Foodie.Services.Restaurant.ViewModels;
using Foodie.Utilities;
using System.Collections.Generic;

namespace Foodie.Apis.Restaurant
{
    public static class RestaurantApi
    {
        public static void RegisterApi(this WebApplication app)
        {
            var root = "api/restaurant/";
            app.MapGet(root + "list/", List);
        }

        private static IResult<ListVM<RestaurantVM>> List(IRestaurantService service, string search = "", int skip = 0, int take = 10)
        {
            return service.List(search.ToLower(), skip, take);
        }
    }
}
