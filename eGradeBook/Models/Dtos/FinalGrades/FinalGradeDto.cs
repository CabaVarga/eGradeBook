using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.FinalGrades
{
    /// <summary>
    /// Dto for final grades
    /// </summary>
    public class FinalGradeDto
    {
        /// <summary>
        /// The student -- only the full name
        /// </summary>
        public string Student { get; set; }

        /// <summary>
        /// The subject (course) -- only the name
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The (school) grade for the final grade
        /// </summary>
        public int SchoolGrade { get; set; }

        /// <summary>
        /// Semester or school term
        /// </summary>
        public int Semester { get; set; }

        /// <summary>
        /// Finally, the grade itself
        /// </summary>
        public int FinalGrade { get; set; }
    }
}