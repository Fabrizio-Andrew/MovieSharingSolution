using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Data;
using HW6MovieSharingSolution.Models;

namespace HW6MovieSharingSolution.Pages.Movies
{
    public class BorrowApproveModel : BasePageModel
    {
        private readonly MyContext _context;

        public BorrowApproveModel(MyContext context) : base(context)
        {
            _context = context;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movie.FirstOrDefaultAsync(m => m.ID == id);

            if (Movie == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostApprove()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Prevent a user without the owner role from approving a borrower
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner != true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            Movie movieToUpdate = await _context.Movie.FindAsync(Movie.ID);

            // Move the requestor's data to SharedWith
            movieToUpdate.SharedWithId = movieToUpdate.RequestorId;
            movieToUpdate.SharedWithName = movieToUpdate.RequestorName;
            movieToUpdate.SharedWithEmailAddress = movieToUpdate.RequestorEmail;
            movieToUpdate.RequestorId = null;
            movieToUpdate.RequestorName = null;
            movieToUpdate.RequestorEmail = null;

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

        public async Task<IActionResult> OnPostDecline()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Prevent a user without the owner role from declining a borrower
            Role role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);
            if (role.Owner != true)
            {
                return StatusCode((int)HttpStatusCode.Forbidden);
            }

            Movie movieToUpdate = await _context.Movie.FindAsync(Movie.ID);

            // Remove the requestor Data
            movieToUpdate.RequestorId = null;
            movieToUpdate.RequestorName = null;
            movieToUpdate.RequestorEmail = null;

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
