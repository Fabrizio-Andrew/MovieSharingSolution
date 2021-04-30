using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Data;
using HW6MovieSharingSolution.Models;

namespace HW6MovieSharingSolution.Pages.Movies
{
    public class ReturnModel : BasePageModel
    {
        private readonly MyContext _context;

        public ReturnModel(MyContext context) : base(context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        /// <summary>
        /// Gets the return page for the specified movie.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The return page.</returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == id);

            // Prevent a user from accessing this page unless the movie is currently shared with them.
            if (Movie.SharedWithId != AuthenticatedUserInfo.ObjectIdentifier)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            // Prevent a user with the owner role from accessing this page
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner == true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            // Return 404 if movie does not exist
            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// Sets the movie's Returned attribute to true.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A redirect to the movies list</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Prevent a user with the owner role from accessing this page
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner == true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            // Get the specified movie entity
            Movie movieToUpdate = await _context.Movie.FindAsync(id);

            // Prevent a user from returning the movie unless it is currently shared with them.
            if (movieToUpdate.SharedWithId != AuthenticatedUserInfo.ObjectIdentifier)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            // Return the movie
            movieToUpdate.Returned = true;
            _context.Attach(movieToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(Movie.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
    }
}
