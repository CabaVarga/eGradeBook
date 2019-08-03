using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.ClassRooms
{
    /// <summary>
    /// Create a new entry in the classroom program
    /// </summary>
    public class ClassRoomProgramDto
    {
        /// <summary>
        /// Classroom id
        /// </summary>
        public int ClassRoomId { get; set; }

        /// <summary>
        /// Course id
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// Teacher id
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// Weekly teaching hours
        /// </summary>
        public int WeeklyHours { get; set; }
    }
}