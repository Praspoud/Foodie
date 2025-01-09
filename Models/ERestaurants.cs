using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("restaurants")]
    public class ERestaurants
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("restaurant_id")]
        public int RestaurantId { get; set; }

        [Column("restaurant_name")]
        [MaxLength(50)]
        public string RestaurantName { get; set; }

        [Column("restaurant_address")]
        [MaxLength(255)]
        public string RestaurantAddress { get; set; }

        [Column("latitude")]
        public decimal Latitude { get; set; }

        [Column("longitude")]
        public decimal Longitude { get; set; }

        //[Column("opening_time")]
        //[MaxLength(10)]
        //public string OpeningTime { get; set; }
        
        //[Column("closing_time")]
        //[MaxLength(10)]
        //public string ClosingTime { get; set; }

        [Column("restaurant_website")]
        [MaxLength(50)]
        public string RestaurantWebsite { get; set; }
        
        [Column("restaurant_contact")]
        [MaxLength(15)]
        public string RestaurantContact { get; set; }

        [Column("restaurant_map_link")]
        public string RestaurantMapLink { get; set; }

        [Column("restaurant_description")]
        [MaxLength(256)]
        public string RestaurantDescription { get; set; }

        [Column("image_url")]
        public string? ImageUrl { get; set; }

    }
}
