namespace Foodie.Services.Restaurant.ViewModels
{
    public class RestaurantVM
    {
        public int Id { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string RestaurantWebsite { get; set; }
        public string RestaurantContact { get; set; }
    }
}
