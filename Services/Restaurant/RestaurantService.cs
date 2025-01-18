using Foodie.Common.Models;
using Foodie.Common.Services;
using Foodie.Models;
using Foodie.Services.Restaurant.ViewModels;
using Foodie.Services.User.ViewModels;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Foodie.Services.Restaurant
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IServiceFactory _factory;
        private readonly IServiceRepository<ERestaurants> _restaurantService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FileUpload _fileUpload;

        public RestaurantService(IHttpContextAccessor httpContextAccessor, IServiceFactory factory, IConfiguration congf, FileUpload fileUpload)
        {
            _factory = factory;
            _restaurantService = _factory.GetInstance<ERestaurants>();
            _configuration = congf;
            _httpContextAccessor = httpContextAccessor;
            _fileUpload = fileUpload;
        }

        public IResult<RestaurantVM> Get(string restaurantId)
        {
            try
            {
                var Restaurants = _restaurantService.FindByName(x => x.RestaurantId == restaurantId);
                var hostAddress = _fileUpload.GetLocalIPAddress();

                if (Restaurants != null)
                {
                    var result = new RestaurantVM
                    {
                        Id = Restaurants.Id,
                        RestaurantId = Restaurants.RestaurantId,
                        RestaurantName = Restaurants.RestaurantName,
                        RestaurantAddress = Restaurants.RestaurantAddress,
                        Latitude = Restaurants.Latitude,
                        Longitude = Restaurants.Longitude,
                        RestaurantWebsite = Restaurants.RestaurantWebsite,
                        RestaurantContact = Restaurants.RestaurantContact,
                        RestaurantMapLink = Restaurants.RestaurantMapLink,
                        RestaurantDescription = Restaurants.RestaurantDescription,
                        ImageUrl = string.IsNullOrWhiteSpace(Restaurants.ImageUrl) ? null : hostAddress + "/" + Restaurants.ImageUrl,
                    };
                    return new IResult<RestaurantVM>
                    {
                        Data = result,
                        Message = "Restaurant retrieved successfully.",
                        Status = ResultStatus.Success
                    };
                }
                return new IResult<RestaurantVM>
                {
                    Message = "Restaurant not found.",
                    Status = ResultStatus.Failure
                };
            }
            catch (Exception ex)
            {
                return new IResult<RestaurantVM>
                {
                    Status = ResultStatus.Failure,
                    Message = "Failed to retrieve Restaurant."
                };
            }
        }

        public IResult<int> Add(RestaurantVM model)
        {
            try
            {
                _factory.BeginTransaction();
                string fileUrl = _fileUpload.CompressAndSaveImage(model.Image, "RestaurantImage").Result;


                var restaurants = new ERestaurants
                {
                    RestaurantId = model.RestaurantId,
                    RestaurantName = model.RestaurantName,
                    RestaurantAddress = model.RestaurantAddress,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    RestaurantWebsite = model.RestaurantWebsite,
                    RestaurantContact = model.RestaurantContact,
                    RestaurantMapLink = model.RestaurantMapLink,
                    RestaurantDescription = model.RestaurantDescription,
                    ImageUrl = fileUrl,
                };

                var result = _restaurantService.Add(restaurants);
                _factory.CommitTransaction();
                return new IResult<int>
                {
                    Data = result.Id,
                    Message = "Restaurant added successfully.",
                    Status = ResultStatus.Success
                };
            }
            catch (Exception ex)
            {
                _factory.RollBack();
                return new IResult<int>
                {
                    Status = ResultStatus.Failure,
                    Message = "Failed to add Restaurant."
                };
            }
        }

        public IResult<ListVM<RestaurantVM>> List(string search, int skip, int take)
        {
            try
            {
                var Restaurants = _restaurantService.List()
                    .Where(x => x.RestaurantName.ToLower().Contains(search.ToLower()));
                var hostAddress = _fileUpload.GetLocalIPAddress();
                var RestaurantsList = Restaurants.Skip(skip).Take(take)
                    .Select(Restaurants => new RestaurantVM
                    {
                        Id = Restaurants.Id,
                        RestaurantId = Restaurants.RestaurantId,
                        RestaurantName = Restaurants.RestaurantName,
                        RestaurantAddress = Restaurants.RestaurantAddress,
                        Latitude = Restaurants.Latitude,
                        Longitude = Restaurants.Longitude,
                        RestaurantWebsite = Restaurants.RestaurantWebsite,
                        RestaurantContact = Restaurants.RestaurantContact,
                        RestaurantMapLink = Restaurants.RestaurantMapLink,
                        RestaurantDescription = Restaurants.RestaurantDescription,
                        ImageUrl = string.IsNullOrWhiteSpace(Restaurants.ImageUrl) ? null : hostAddress + "/" + Restaurants.ImageUrl,
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
