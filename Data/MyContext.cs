using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Models;


namespace HW6MovieSharingSolution.Data
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions<MyContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Movie> Movie { get; set; }

        public DbSet<Role> Role { get; set; }
    }
}
