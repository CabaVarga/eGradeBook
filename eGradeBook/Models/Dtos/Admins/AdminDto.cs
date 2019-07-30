using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Admins
{
    /// <summary>
    /// Dto object with info about an admin user
    /// </summary>
    public class AdminDto
    {
        /// <summary>
        /// Admin Id of type int
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Full name -- First name + space + Last name
        /// </summary>
        public string FullName { get; set; }
    }
}