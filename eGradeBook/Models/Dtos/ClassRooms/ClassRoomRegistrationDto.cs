using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.ClassRooms
{
    /// <summary>
    /// Used for registering a classroom in the system.
    /// The current model is quite poor, needs to be expanded.
    /// </summary>
    public class ClassRoomRegistrationDto
    {
        /// <summary>
        /// A classroom always have a name, 5A etc
        /// </summary>
        [Required]
        [StringLength(4, ErrorMessage = "Classroom name should be between 2 and 4 characters.", MinimumLength = 2)]
        public string Name { get; set; }

        /// <summary>
        /// A classroom belongs to a given grade. Grade must be in a range.
        /// </summary>
        [Required]
        [Range(1, 8, ErrorMessage = "Acceptable grades are 1st to 8th.")]
        public int SchoolGrade { get; set; }
    }
}