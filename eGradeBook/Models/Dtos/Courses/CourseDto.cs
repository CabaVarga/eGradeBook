using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Courses
{
    /// <summary>
    /// Basic informations about a course
    /// </summary>
    public class CourseDto
    {
        /// <summary>
        /// Course Id
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// Course name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Colloqial name of the course
        /// </summary>
        public string ColloqialName { get; set; }
    }
}