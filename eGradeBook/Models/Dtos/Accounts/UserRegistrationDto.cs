using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Accounts
{
    /// <summary>
    /// User registration base class
    /// </summary>
    public abstract class UserRegistrationDto
    {
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

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Password confirmation
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}