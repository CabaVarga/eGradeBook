using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Teachers
{
    /// <summary>
    /// Teacher Dto
    /// </summary>
    public class TeacherDto
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TeacherDto()
        {
            this.Courses = new List<CourseList>();
        }

        /// <summary>
        /// A list of courses taught by the teacher
        /// </summary>
        public class CourseList
        {
            /// <summary>
            /// Course Id
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Course name
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// The teachers full name        
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The teachers Id
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// A list of courses taught by the teacher
        /// </summary>
        public List<CourseList> Courses { get; set; }
    }
}