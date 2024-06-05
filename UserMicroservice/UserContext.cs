using Microsoft.EntityFrameworkCore;
using UserMicroservice.Models;


namespace UserMicroservice
{
    // Data/UserContext.cs
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Ensure this is correctly set
        }
    }


}
