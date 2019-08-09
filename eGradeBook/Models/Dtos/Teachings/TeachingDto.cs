using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Teachings
{
    public class TeachingDto
    {
        /// <summary>
        /// Teaching Id
        /// </summary>
        public int TeachingId { get; set; }

        /// <summary>
        /// Course Id
        /// </summary>
        [Required]
        public int CourseId { get; set; }

        /// <summary>
        /// Teacher Id
        /// </summary>
        [Required]
        public int TeacherId { get; set; }

        /// <summary>
        /// Course name
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// Teachers username
        /// </summary>
        public string TeacherName { get; set; }
    }
}