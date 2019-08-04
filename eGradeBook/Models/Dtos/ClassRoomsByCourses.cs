using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    /// <summary>
    /// ClassRooms grouped by courses being taught in them
    /// </summary>
    public class ClassRoomsByCourses
    {
        /// <summary>
        /// The course
        /// </summary>
        public string Course { get; set; }

        // This is same as programs by .. only without the details....

        /// <summary>
        /// ClassRooms
        /// </summary>
        public ICollection<string> ClassRooms { get; set; }
    }
}