using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Accounts
{
    /// <summary>
    /// Registration data structure for Parent users
    /// </summary>
    public class ParentRegistrationDto : UserRegistrationDto
    {
        /// <summary>
        /// E-mail address
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "E-mail address")]
        public new string Email { get; set; }
    }
}