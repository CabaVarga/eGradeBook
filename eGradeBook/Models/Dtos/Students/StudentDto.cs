using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Students
{
    /// <summary>
    /// Data transfer object for Student users
    /// NOTE: we will probably need more than one... with different properties.
    /// </summary>
    public class StudentDto
    {
        /// <summary>
        /// User name
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// E-mail address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Place of birth
        /// </summary>
        public string PlaceOfBirth { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Class room
        /// </summary>
        public string ClassRoom { get; set;}

        /// <summary>
        /// Student Id
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Class room Id
        /// </summary>
        public int? ClassRoomId { get; set; }

        public int? AvatarId { get; set; }
    }
}