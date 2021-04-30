using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Data;
using HW6MovieSharingSolution.Models;
using HW6MovieSharingSolution.Pages;

namespace HW6MovieSharingSolution.Pages
{
    public class IndexModel : BasePageModel
    {
        //private readonly MyContext _context;

        public IndexModel(MyContext context) : base(context)
        {
        //    _context = context;
        }

        public Role Role { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Role = await Context.Role.SingleOrDefaultAsync(m => m.ID == AuthenticatedUserInfo.ObjectIdentifier);

            // Create a role for the user if one does not already exist
            if (Role == null)
            {
                Role newRole = new Role
                {
                    ID = AuthenticatedUserInfo.ObjectIdentifier,
                    Owner = false
                };
                Context.Role.Add(newRole);
                Context.SaveChanges();
            }

            // Redirect to Movies List
            return RedirectToPage("./Movies/Index");

        }
    }
}
