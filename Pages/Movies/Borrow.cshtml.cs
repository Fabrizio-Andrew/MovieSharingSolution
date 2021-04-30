using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Data;
using HW6MovieSharingSolution.Models;
using System.Security.Claims;

namespace HW6MovieSharingSolution.Pages.Movies
{
    public class BorrowModel : BasePageModel
    {
        private readonly MyContext _context;

        public BorrowModel(MyContext context) : base(context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        /// <summary>
        /// Gets the Borrow page for the specified Movie.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The Borrow Page</returns>
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == id);

            // Prevent a user from accessing the borrow page for a movie that is currently shared or not shareable.
            if (Movie.IsSharable == false || Movie.SharedWithId != null)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            // Prevent a user with the owner role from accessing this page
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner == true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            // Return if Movie does not exist
            if (Movie == null)
            {
                return NotFound();
            }

            return Page();
        }

        /// <summary>
        /// Updates the Borrow attributes for the specified Movie.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Redirect to the Movie List</returns>
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Movie movieToUpdate = await _context.Movie.FindAsync(id);

            // Prevent a user from requesting to borrow a movie that is currently shared or not shareable.
            if (movieToUpdate.IsSharable == false || movieToUpdate.SharedWithId != null)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            // Prevent a user with the owner role from requesting to borrow a movie
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner == true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            if (movieToUpdate == null)
            {
                return NotFound();
            }

            // Get the claims for the authenticated user
            ClaimsPrincipal cp = this.User;
            var claims = cp.Claims.ToList();

            // Update the requestor info based on the user's claims
            movieToUpdate.RequestorName = claims?.FirstOrDefault(x => x.Type.Equals("name", StringComparison.OrdinalIgnoreCase))?.Value;
            movieToUpdate.RequestorEmail = claims?.FirstOrDefault(x => x.Type.Equals("preferred_username", StringComparison.OrdinalIgnoreCase))?.Value;
            movieToUpdate.RequestorId = claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier", StringComparison.OrdinalIgnoreCase))?.Value;

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

            return RedirectToPage("./Movies/Index");
        }


        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.ID == id);
        }
    }
}
