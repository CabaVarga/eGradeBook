using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    /// <summary>
    /// Programs of every classroom
    /// </summary>
    public class ProgramsBySchoolClassesDto
    {
        /// <summary>
        /// ClassRoom
        /// </summary>
        public string ClassRoom { get; set; }

        /// <summary>
        /// List of courses being taught in the classroom
        /// Not enough data, by far...
        /// TODO change the model, ffs!
        /// </summary>
        public ICollection<string> Courses { get; set; }
    }

    /// <summary>
    /// I don't even remember if I'm using this
    /// A little more detailed description of the course in the classroom's program
    /// </summary>
    public class ProgramsBySchoolClassesDetail
    {
        /// <summary>
        /// Course name
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Grade from 1 to 8
        /// </summary>
        public int Grade { get; set; }

        /// <summary>
        /// Weekly teaching hours
        /// </summary>
        public int WeeklyHours { get; set; }

        /// <summary>
        /// The teacher for the course
        /// </summary>
        public string Teacher { get; set; }
    }
}