using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Models;
using HW6MovieSharingSolution.Data;
using HW6MovieSharingSolution.Pages;


namespace HW6MovieSharingSolution.Pages.Movies
{
    public class IndexModel : BasePageModel
    {
        private readonly MyContext _context;

        public IndexModel(MyContext context) : base(context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; }

        public Role role { get; set; }

        public async Task OnGetAsync()
        {
            role = await _context.Role.SingleOrDefaultAsync(x => x.ID == AuthenticatedUserInfo.ObjectIdentifier);

            if (role.Owner == true)
            {
                Movie = await _context.Movie.Where(_ => _.OwnerId == role.ID).ToListAsync();
            }
            else
            {
                Movie = await _context.Movie.Where(_ => _.IsSharable == true).ToListAsync();
            }
        }
    }
}
