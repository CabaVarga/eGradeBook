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
        /// Grade Id
        /// </summary>
        public int GradeId { get; set; }

        /// <summary>
        /// Course Id
        /// </summary>
        public int CourseId { get; set; }

        /// <summary>
        /// Teacher Id
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// ClassRoom Id
        /// </summary>
        public int ClassRoomId { get; set; }

        /// <summary>
        /// Student Id
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// School term
        /// </summary>
        public int SchoolTerm { get; set; }

        /// <summary>
        /// Assignment date
        /// </summary>
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