using System;
using System.Collections.Generic;
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
        public string Name { get; set; }

        /// <summary>
        /// A classroom belongs to a given grade. Grade must be in a range.
        /// </summary>
        public int SchoolGrade { get; set; }
    }
}