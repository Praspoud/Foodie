using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("restaurant_ratings")]
    public class ERestaurantRatings
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("post_id")]
        [ForeignKey("UserPosts")]
        public int PostId { get; set; }

        [Column("user_id")]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Column("restaurant_id")]
        [ForeignKey("Restaurants")]
        public string RestaurantId { get; set; }

        [Column("rating_type")]
        public string RatingType { get; set; }

        [Column("score")]
        public int? Score { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual EUserPosts UserPosts { get; set; }
        public virtual EUsers Users { get; set; }
    }
}
