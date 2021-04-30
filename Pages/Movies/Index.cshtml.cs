using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Models;
using HW6MovieSharingSolution.Data;

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

        /// <summary>
        /// Returns the /Movies/Index page with a list of movies based on the user's role.
        /// </summary>
        /// <returns>The /Movies/Index page.</returns>
        public async Task OnGetAsync()
        {
            role = await _context.Role.SingleOrDefaultAsync(x => x.ID == AuthenticatedUserInfo.ObjectIdentifier);

            if (role.Owner == true)
            {
                // Display all movies to anyone with the "owner" role
                Movie = await _context.Movie.ToListAsync();
            }
            else
            {
                // Display only sharable movies to authenticated users without the "owner" role
                Movie = await _context.Movie.Where(_ => _.IsSharable == true).ToListAsync();
            }
        }
    }
}
