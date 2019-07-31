using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    /// <summary>
    /// Used for listing programs, grouped by courses
    /// </summary>
    public class ProgramsByCoursesDto
    {
        /// <summary>
        /// The course name
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Class rooms where the course is in their program, only their names
        /// </summary>
        public ICollection<string> ClassRooms { get; set; }
    }

    /// <summary>
    /// Details about the classroom, can be used instead of a simple string list of classrooms
    /// NOTE: Not used anywhere
    /// </summary>
    public class ProgramsByCoursesDetail
    {
        /// <summary>
        /// Classroom name
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Classroom grade
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// How many teaching hours per week for the course?
        /// </summary>
        public int WeeklyHours { get; set; }

        /// <summary>
        /// Who is the teacher teaching the course in the given classroom?
        /// </summary>
        public string Teacher { get; set; }
    }

}