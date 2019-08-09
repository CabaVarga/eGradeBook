using eGradeBook.Utilities.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        /// Grade Id
        /// </summary>
        public int GradeId { get; set; }

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
        /// ClassRoom Id
        /// </summary>
        [Required]
        public int ClassRoomId { get; set; }

        /// <summary>
        /// Student Id
        /// </summary>
        [Required]
        public int StudentId { get; set; }

        /// <summary>
        /// School term
        /// </summary>
        [Required]
        [Range(1, 2, ErrorMessage = "Semester must be 1 or 2")]
        public int SchoolTerm { get; set; }

        /// <summary>
        /// Assignment date
        /// </summary>
        [Required]
        [JsonConverter(typeof(DateFormatConverter), "yyyy - MM - dd")]
        public DateTime AssignmentDate { get; set; }

        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Subject (course) name
        /// </summary>
        public string Course { get; set; }

        /// <summary>
        /// Grade point
        /// </summary>
        [Required]
        [Range(1, 5, ErrorMessage = "Grade point must be in range 1 to 5")]
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