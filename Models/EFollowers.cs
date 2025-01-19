using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("followers")]
    public class EFollowers
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("follower_id")]
        [ForeignKey("FollowerUsers")]
        public int FollowerId { get; set; }

        [Column("followee_id")]
        [ForeignKey("FolloweeUsers")]
        public int FolloweeId { get; set; }

        [Column("followed_at")]
        public DateTime FollowedAt { get; set; }

        public EUsers FollowerUsers { get; set; }
        public EUsers FolloweeUsers { get; set; }
    }
}
