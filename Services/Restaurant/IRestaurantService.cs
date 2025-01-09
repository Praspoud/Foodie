using Foodie.Common.Models;
using Foodie.Services.Restaurant.ViewModels;

namespace Foodie.Services.Restaurant
{
    public interface IRestaurantService
    {
        IResult<int> Add(RestaurantVM model);
        IResult<ListVM<RestaurantVM>> List(string search, int skip, int take);
    }
}
