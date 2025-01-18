using Foodie.Common.Models;
using Foodie.Models;
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
            app.MapGet(root, Get);
            app.MapGet(root + "list/", List);
        }
        private static IResult<int> Add(HttpRequest request, IRestaurantService service, IFoodieSessionAccessor accessor, int RestaurantId, string Name, string Address, decimal Longitude, decimal Latitude, string Website, string Contact, string MapLink, string Description)
        {
            RestaurantVM model = new();
            model.Image = (request.Form.Files.Count > 0) ? request.Form.Files[0] : null;
            model.RestaurantId = RestaurantId;
            model.Longitude = Longitude;
            model.Latitude = Latitude;
            model.RestaurantName = Name;
            model.RestaurantAddress = Address;
            model.RestaurantWebsite = Website;
            model.RestaurantContact = Contact;
            model.RestaurantMapLink = MapLink;
            model.RestaurantDescription = Description;
            return service.Add(model);
        }
        private static IResult<RestaurantVM> Get(IRestaurantService service, int Id)
        {
            return service.Get(Id);
        }

        private static IResult<ListVM<RestaurantVM>> List(IRestaurantService service, string search = "", int skip = 0, int take = 10)
        {
            return service.List(search.ToLower(), skip, take);
        }


    }
}
