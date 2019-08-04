using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Grades
{
    /// <summary>
    /// Dto for grades, limited for now
    /// For viewing, not for grade assignment
    /// </summary>
    public class GradeDto
    {
        /// <summary>
        /// Subject (course) name
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Grade point
        /// </summary>
        public int GradePoint { get; set; }

        /// <summary>
        /// The student's full name
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// The teacher's full name
        /// </summary>
        public string TeacherName { get; set; }
    }
}