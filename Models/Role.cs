using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HW6MovieSharingSolution.Models
{
    public class Role
    {
        /// <summary>
        /// Gets or sets the UserID.
        /// </summary>
        /// <value>The User's ID.</value>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or set the Owner indicator.
        /// </summary>
        /// <value>The User's Owner indicator.</value>
        public bool Owner { get; set; }
    }
}
