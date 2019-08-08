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
        /// Final grade Id
        /// </summary>
        public int FinalGradeId { get; set; }

        /// <summary>
        /// The student -- only the Id
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// The subject (course) -- only the id
        /// </summary>
        public int CourseId { get; set; }

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