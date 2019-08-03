using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Utilities.Common
{
    /// <summary>
    /// Used to categorize user
    /// </summary>
    public enum UserRole
    {
        /// <summary>
        /// An admin user
        /// </summary>
        ADMIN,
        /// <summary>
        /// A teacher user
        /// </summary>
        TEACHER,
        /// <summary>
        /// A student user
        /// </summary>
        STUDENT,
        /// <summary>
        /// A parent user
        /// </summary>
        PARENT
    }
}