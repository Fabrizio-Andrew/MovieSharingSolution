using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Data;
using HW6MovieSharingSolution.Models;

namespace HW6MovieSharingSolution.Pages.Movies
{
    public class ReturnApproveModel : BasePageModel
    {
        private readonly MyContext _context;

        public ReturnApproveModel(MyContext context) : base(context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        /// <summary>
        /// Gets the Return Approval page for the specified movie.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Return Approval page.</returns>
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

            // Return 404 if the movie does not exist.
            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// Sets the movie's "Shared" attributes to null and the Returned indicator to false.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A redirect to the movies list.</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Prevent a user without the owner role from accepting a return
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner != true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            // Get the specified movie entity
            Movie movieToUpdate = await _context.Movie.FindAsync(id);

            // Accept the returned movie
            movieToUpdate.Returned = false;
            movieToUpdate.SharedWithId = null;
            movieToUpdate.SharedWithName = null;
            movieToUpdate.SharedWithEmailAddress = null;
            movieToUpdate.SharedDate = null;

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
