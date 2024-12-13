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

        [Column("restaurant_name")]
        [MaxLength(50)]
        public string RestaurantName { get; set; }

        //[Column("restaurant_website")]
        //[MaxLength(50)]
        //public string RestaurantWebsite { get; set; }

    }
}
