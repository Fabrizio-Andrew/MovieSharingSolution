using Microsoft.Graph;
using System;
using System.IO;
using System.Security.Claims;

namespace HW6MovieSharingSolution
{

    // Helper methods to access Graph user data stored in
    // the claims principal
    public static class ClaimsPrincipalExtensions
    {

        /// <summary>
        /// Objects the identifier.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal.</param>
        /// <returns>System.String.</returns>
        public static string ObjectIdentifier(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value ?? string.Empty;
        }

        public static string DisplayName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirst("name")?.Value ?? string.Empty;
        }
    }
    /// <summary>
    /// Provides access to claims values
    /// </summary>
    public class DecodedClaims
    {
        /// <summary>
        /// The claims principal
        /// </summary>
        private readonly ClaimsPrincipal _claimsPrincipal;

        public DecodedClaims(ClaimsPrincipal claimsPrincipal)
        {
            _claimsPrincipal = claimsPrincipal;
        }       

        /// <summary>
        /// Gets the object identifier.
        /// </summary>
        /// <value>The object identifier.</value>
        public string ObjectIdentifier
        {
            get
            {
                return _claimsPrincipal.ObjectIdentifier();
            }
        }

    }
}