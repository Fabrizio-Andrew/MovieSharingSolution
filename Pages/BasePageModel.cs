using HW6MovieSharingSolution.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HW6MovieSharingSolution.Pages
{
    public class BasePageModel : PageModel
    {
        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        protected MyContext Context { get; }

        /// <summary>
        /// The decoded claims
        /// </summary>
        private DecodedClaims _decodedClaims = null;

        /// <summary>
        /// Gets the authenticated user information.
        /// </summary>
        /// <value>The authenticated user information.</value>
        public DecodedClaims AuthenticatedUserInfo
        {
            get
            {
                if (_decodedClaims == null)
                {
                    _decodedClaims = new DecodedClaims(this.User);
                }
                return _decodedClaims;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePageModel"/> class.
        /// </summary>
        public BasePageModel(MyContext context)
        {
            Context = context;
        }

    }
}

