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
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        /// <summary>
        /// The user's gender.
        /// </summary>
        [Required]
        [MaxLength(1)]
        [RegularExpression("m|f", ErrorMessage = "Acceptable values are \"m\" for Male and \"f\" for Female.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        [DataType(DataType.EmailAddress, ErrorMessage = "Provided email cannot be validated")]
        [EmailAddress(ErrorMessage = "Bad email format")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        [Display(Name = "Phone number")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Not a valid phone number")]
        public string PhoneNumber { get; set; }

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