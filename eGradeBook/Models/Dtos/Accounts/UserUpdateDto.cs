using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Accounts
{
    /// <summary>
    /// User update base class
    /// </summary>
    public abstract class UserUpdateDto
    {
        /// <summary>
        /// Id of user
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [Required]
        public string LastName { get; set; }


    }
}