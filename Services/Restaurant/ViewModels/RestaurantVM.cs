namespace Foodie.Services.Restaurant.ViewModels
{
    public class RestaurantVM
    {
        public int Id { get; set; }
        public string RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantAddress { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string RestaurantWebsite { get; set; }
        public string RestaurantContact { get; set; }
        public string RestaurantMapLink { get; set; }
        public string RestaurantDescription { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
