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
            app.MapPost(root, Add);
            app.MapGet(root + "list/", List);
        }
        private static IResult<int> Add(HttpRequest request, IRestaurantService service, IFoodieSessionAccessor accessor, RestaurantVM model)
        {
            //RestaurantVM model = new();
            //model.Image = (request.Form.Files.Count > 0) ? request.Form.Files[0] : null;
            //model.Longitude = Longitude;
            //model.Latitude = Latitude;
            //model.RestaurantName = Name;
            //model.RestaurantAddress = Address;
            return service.Add(model);
        }

        private static IResult<ListVM<RestaurantVM>> List(IRestaurantService service, string search = "", int skip = 0, int take = 10)
        {
            return service.List(search.ToLower(), skip, take);
        }


    }
}
