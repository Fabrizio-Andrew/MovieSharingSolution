using System;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HW6MovieSharingSolution.Models;
using HW6MovieSharingSolution.Data;

namespace HW6MovieSharingSolution.Pages.Movies
{
    public class CreateModel : BasePageModel
    {
        private readonly MyContext _context;

        public CreateModel(MyContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a blank Create Page.
        /// </summary>
        /// <returns>The Create Page.</returns>
        public async Task<IActionResult> OnGet()
        {
            // Prevent a user without the owner role from accessing this page
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner != true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; }

        /// <summary>
        /// Creates a new movie based on client input.
        /// </summary>
        /// <returns>A redirect to the movies list.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Prevent a user without the owner role from creating a movie
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner != true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            Movie newMovie = new Movie();

            // Set the OwnerID to the currently authenticated user's ID
            ClaimsPrincipal cp = this.User;
            var claims = cp.Claims.ToList();
            newMovie.OwnerId = claims?.FirstOrDefault(x => x.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier", StringComparison.OrdinalIgnoreCase))?.Value;

            // Set Title/Category/IsSharable attributes from form input
            await TryUpdateModelAsync<Movie>(newMovie, "Movie", s => s.Title, s => s.Category, s => s.IsSharable);

            _context.Attach(newMovie);

            await _context.SaveChangesAsync();
            return RedirectToPage("..Movies/Index");
        }
    }
}
