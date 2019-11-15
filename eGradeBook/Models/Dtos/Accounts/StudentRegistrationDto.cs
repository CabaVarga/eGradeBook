using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Accounts
{
    /// <summary>
    /// Registration data structure for Student users
    /// </summary>
    public class StudentRegistrationDto : UserRegistrationDto
    {
        /// <summary>
        /// Place of birth. Examples: Novi Sad, Belgrade, Becej
        /// </summary>
        public string PlaceOfBirth { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        public int ClassRoomId { get; set; }
    }
}