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
    }
}
