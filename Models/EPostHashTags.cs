using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("post_hash_tags")]
    public class EPostHashTags
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("post_id")]
        [ForeignKey("UserPosts")]
        public int PostId { get; set; }

        [Column("hash_tag_id")]
        [ForeignKey("HashTags")]
        public int HashTagId { get; set; }

        public virtual EUserPosts UserPosts { get; set; }
        public virtual EHashTags HashTags { get; set; }
    }
}
