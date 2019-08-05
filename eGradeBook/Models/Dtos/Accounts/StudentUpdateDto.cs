using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Accounts
{
    /// <summary>
    /// Update personal details of student user
    /// </summary>
    public class StudentUpdateDto : UserUpdateDto
    {
        /// <summary>
        /// Place of birth. Examples: Novi Sad, Belgrade, Becej
        /// </summary>
        public string PlaceOfBirth { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
    }
}