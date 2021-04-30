using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HW6MovieSharingSolution.Data;
using HW6MovieSharingSolution.Models;

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

        /// <summary>
        /// Creates a Role entity for the user if one does not already exist.
        /// Redirects the user to the /Movies/Index page.
        /// </summary>
        /// <returns>A redirect to the movies list.</returns>
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
