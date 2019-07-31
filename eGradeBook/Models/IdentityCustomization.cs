using eGradeBook.Infrastructure;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// User role with int keys
    /// </summary>
    public class CustomUserRole : IdentityUserRole<int> { }

    /// <summary>
    /// User claim with int keys
    /// </summary>
    public class CustomUserClaim : IdentityUserClaim<int> { }

    /// <summary>
    /// User login (external logins) with int keys
    /// </summary>
    public class CustomUserLogin : IdentityUserLogin<int> { }

    /// <summary>
    /// Identity role with int key
    /// </summary>
    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        /// <summary>
        /// Empty constructor
        /// </summary>
        public CustomRole() { }

        /// <summary>
        /// Constructor with role name
        /// </summary>
        /// <param name="name"></param>
        public CustomRole(string name) { Name = name; }
    }

    /// <summary>
    /// User store implementation with int keys
    /// </summary>
    public class CustomUserStore : UserStore<GradeBookUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public CustomUserStore(GradeBookContext context)
            : base(context)
        {
        }
    }

    /// <summary>
    /// Role store implementation with int keys
    /// </summary>
    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="context"></param>
        public CustomRoleStore(GradeBookContext context)
            : base(context)
        {
        }
    }
}