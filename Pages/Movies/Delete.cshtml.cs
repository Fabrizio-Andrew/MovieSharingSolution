using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Models;
using HW6MovieSharingSolution.Data;

namespace HW6MovieSharingSolution.Pages.Movies
{
    public class DeleteModel : BasePageModel
    {
        private readonly MyContext _context;

        public DeleteModel(MyContext context) : base(context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        /// <summary>
        /// Gets the delete page for the specified movie.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The delete page.</returns>
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

            // Return 404 is movie does not exist.
            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        /// <summary>
        /// Deletes the specified movie from the DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A redirect to the movies list.</returns>
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Prevent a user without the owner role from deleting a movie
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner != true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            Movie = await _context.Movie.FindAsync(id);

            // Delete the movie
            if (Movie != null)
            {
                _context.Movie.Remove(Movie);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Movies/Index");
        }
    }
}
