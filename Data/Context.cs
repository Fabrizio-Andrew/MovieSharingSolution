using Microsoft.EntityFrameworkCore;

namespace HW6MovieSharingSolution.Data
{
    public class AzureADEFRazorPagesDemo2Context : DbContext
    {
        public AzureADEFRazorPagesDemo2Context (DbContextOptions<AzureADEFRazorPagesDemo2Context> options)
            : base(options)
        {
            // This will cause the Database and Tables to be created.
            Database.EnsureCreated();
        }

        public DbSet<HW6MovieSharingSolution.Models.Movie> Movie { get; set; }
    }
}