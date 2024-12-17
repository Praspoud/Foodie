using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.Restaurant.ViewModels;
using Foodie.Services.User.ViewModels;

namespace Foodie.Services.Restaurant
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<ERestaurants> _restaurantService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RestaurantService(IHttpContextAccessor httpContextAccessor, IServiceFactory factory, IConfiguration congf)
        {
            _factory = factory;
            _restaurantService = _factory.GetInstance<ERestaurants>();
            _configuration = congf;
            _httpContextAccessor = httpContextAccessor;
        }
        public IResult<ListVM<RestaurantVM>> List(string search, int skip, int take)
        {
            try
            {
                var Restaurants = _restaurantService.List()
                    .Where(x => x.RestaurantName.ToLower().Contains(search.ToLower()));
                var RestaurantsList = Restaurants.Skip(skip).Take(take)
                    .Select(Restaurants => new RestaurantVM
                    {
                        Id = Restaurants.Id,
                        RestaurantName = Restaurants.RestaurantName,
                        RestaurantAddress = Restaurants.RestaurantAddress,
                        Latitude = Restaurants.Latitude,
                        Longitude = Restaurants.Longitude,
                        RestaurantWebsite = Restaurants.RestaurantWebsite,
                        RestaurantContact = Restaurants.RestaurantContact,
                    }).ToList();

                return new IResult<ListVM<RestaurantVM>>
                {
                    Data = new ListVM<RestaurantVM>
                    {
                        List = RestaurantsList,
                        Count = Restaurants.Count()
                    },
                    Message = "Restaurants retrieved successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                return new IResult<ListVM<RestaurantVM>>
                {
                    Status = ResultStatus.Failure,
                    Message = "Failed to retrieve charging stations."
                };
            }
        }
    }
}
