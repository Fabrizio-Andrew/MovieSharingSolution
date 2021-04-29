using System;
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

        public async Task<IActionResult> OnPostAsyncApprove()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Move the requestor's data to SharedWith
            Movie.SharedWithId = Movie.RequestorId;
            Movie.SharedWithName = Movie.RequestorName;
            Movie.SharedWithEmailAddress = Movie.RequestorEmail;
            Movie.RequestorId = null;
            Movie.RequestorName = null;
            Movie.RequestorEmail = null;

            // Not sure about this line
            _context.Attach(Movie).State = EntityState.Modified;

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

        public async Task<IActionResult> OnPostAsyncDecline()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            // Remove the requestor Data
            Movie.RequestorId = null;
            Movie.RequestorName = null;
            Movie.RequestorEmail = null;

            // Not sure about this line
            _context.Attach(Movie).State = EntityState.Modified;

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
