using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Models;
using HW6MovieSharingSolution.Data;


namespace HW6MovieSharingSolution.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly MyContext _context;

        public IndexModel(MyContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }

        public async Task OnGetAsync()
        {
            Movie = await _context.Movie.ToListAsync();
        }
    }
}
