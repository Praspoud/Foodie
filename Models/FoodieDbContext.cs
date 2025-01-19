using Microsoft.EntityFrameworkCore;

namespace Foodie.Models
{
    public class FoodieDbContext : DbContext
    {
        public FoodieDbContext(DbContextOptions<FoodieDbContext> db) : base(db)
        {

        }

        public FoodieDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<EUsers> Users { get; set; }
        public DbSet<EPasswordHist> PasswordHist { get; set; }
        public DbSet<ERestaurants> Restaurants { get; set; }
        public DbSet<EUsersBio> UsersBio { get; set; }
        //public DbSet<EOtp> Otp { get; set; }
        public DbSet<EUserPosts> UserPosts { get; set; }
        public DbSet<EUserTags> UserTags { get; set; }
        public DbSet<EHashTags> HashTags { get; set; }
        public DbSet<EPostHashTags> PostHashTags { get; set; }
        public DbSet<EPostLikes> PostLikes { get; set; }
        public DbSet<EPostComments> PostComments { get; set; }
    }
}
