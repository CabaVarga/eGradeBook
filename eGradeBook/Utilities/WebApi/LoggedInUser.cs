using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Utilities.WebApi
{
    /// <summary>
    /// Basic info about the authenticated user
    /// NOTE used for logging
    /// </summary>
    public class LoggedInUser
    {
        /// <summary>
        /// In case an unauthenticated user is using the API
        /// We must leave this as nullable, for those cases.
        /// Although -1 would maybe fit too?
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// The users role.
        /// NOTE think about changing it to the Enum
        /// </summary>
        public string UserRole { get; set; }
    }
}