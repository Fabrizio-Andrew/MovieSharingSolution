using HW6MovieSharingSolution.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW6MovieSharingSolution.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MyContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MyContext>>()))
            {
                // Look for any customers.
                if (context.Movie.Any())
                {
                    return;   // DB has been seeded
                }

                context.Movie.AddRange(
                    new Movie
                    {
                        Title = "Star Wars IV",
                        Category = "Science Fiction",
                        IsSharable = true
                    },
                    new Movie
                    {
                        Title = "The Matrix",
                        Category = "Science Fiction",
                        IsSharable = true
                    },                    
                    new Movie
                    {
                        Title = "The Social Network",
                        Category = "Drama",
                        IsSharable = true
                    },                    
                    new Movie
                    {
                        Title = "Star Trek First Contact",
                        Category = "Science Fiction",
                        IsSharable = true
                    },                    
                    new Movie
                    {
                        Title = "Captain Marvel",
                        Category = "Adventure",
                        IsSharable = false
                    });
                context.SaveChanges();
            }
        }

    }
}