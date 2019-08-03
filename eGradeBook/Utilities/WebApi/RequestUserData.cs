using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Utilities.WebApi
{
    /// <summary>
    /// Request data
    /// </summary>
    public class RequestUserData
    {
        /// <summary>
        /// Is the user an admin?
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Is the user a teacher?
        /// </summary>
        public bool IsTeacher { get; set; }

        // User data 
        /// <summary>
        /// Is the user a student?
        /// </summary>
        public bool IsStudent { get; set; }

        /// <summary>
        /// Is the user a parent?
        /// </summary>
        public bool IsParent { get; set; }

        /// <summary>
        /// Is the user authenticated, at all?
        /// </summary>
        public bool IsAuthenticated { get; set; }

        /// <summary>
        /// What is the user's email?
        /// </summary>
        public string UserEmail { get; set; }

        /// <summary>
        /// What is the user's id?
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// What is the user's role?
        /// </summary>
        public string UserRole { get; set; }
    }
}