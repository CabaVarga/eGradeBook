using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    /// <summary>
    /// List courses by classrooms
    /// </summary>
    public class CoursesBySchoolClassesDto
    {
        /// <summary>
        /// The classroom's name
        /// </summary>
        public string ClassRoom { get; set; }

        /// <summary>
        /// The collection of courses in the classroom's program
        /// </summary>
        public ICollection<string> Courses { get; set; }
    }
}