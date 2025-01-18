using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foodie.Models
{
    [Table("hash_tags")]
    public class EHashTags
    {

        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("tag")]
        [MaxLength(255)]
        public string Tag { get; set; }
    }
}
