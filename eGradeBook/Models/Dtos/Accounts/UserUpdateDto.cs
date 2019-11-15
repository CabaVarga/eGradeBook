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

        /// <summary>
        /// The user's gender.
        /// </summary>
        [Required]
        [MaxLength(1)]
        [RegularExpression("m|f", ErrorMessage = "Acceptable values are \"m\" for Male and \"f\" for Female.")]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int? AvatarId { get; set; }


    }
}