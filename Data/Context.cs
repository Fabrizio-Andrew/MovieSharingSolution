using Microsoft.EntityFrameworkCore;

namespace HW6MovieSharingSolution.Data
{
    public class HW6MovieSharingSolutionContext : DbContext
    {
        public HW6MovieSharingSolutionContext (DbContextOptions<HW6MovieSharingSolutionContext> options)
            : base(options)
        {
            // This will cause the Database and Tables to be created.
            Database.EnsureCreated();
        }

        public DbSet<HW6MovieSharingSolution.Models.Movie> Movie { get; set; }
    }
}