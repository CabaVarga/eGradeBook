using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace eGradeBook.Models
{
    /// <summary>
    /// The blueprint for our user models.
    /// Possible additions: phone number(s), address
    /// </summary>
    public abstract class GradeBookUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        /// <summary>
        /// First name of the user
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the user
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        /// <summary>
        /// The user's gender. The current "m" and "f" is very limited.
        /// </summary>
        [MaxLength(1)]
        [RegularExpression("m|f", ErrorMessage = "Acceptable values are \"m\" for Male and \"f\" for Female.")]
        public string Gender { get; set; }

        /// <summary>
        /// We will be probably using this later on the Front End
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<GradeBookUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}