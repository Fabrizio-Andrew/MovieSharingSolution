using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Models;
using HW6MovieSharingSolution.Data;

namespace HW6MovieSharingSolution.Pages.Movies
{
    public class EditModel : BasePageModel
    {
        private readonly MyContext _context;

        public EditModel(MyContext context) : base(context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        /// <summary>
        /// Gets the edit page for the specified movie.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The edit page.</returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Prevent a user without the owner role from accessing this page
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner != true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            Movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == id);

            // Return 404 if the movie does not exist
            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// Updates the movie's Title, Category, and IsSharable attributes based on user input.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A redirect to the movies list.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Prevent a user without the owner role from editing the movie
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner != true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            // Get the Specified movie Entity
            var movieToUpdate = await _context.Movie.FindAsync(id);

            if (movieToUpdate == null)
            {
                return NotFound();
            }

            // Update Title, Category, & IsSharable attributes based on form input
            if (await TryUpdateModelAsync<Movie>(movieToUpdate, "Movie", s => s.Title, s => s.Category, s => s.IsSharable))
            {
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

            return Page();
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
    }
}
