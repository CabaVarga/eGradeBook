using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    /// <summary>
    /// The root object in this complex structure.
    /// </summary>
    public class GradesByCourseDto
    {
        /// <summary>
        /// Course name
        /// </summary>
        public string Course { get; set; }

        /// <summary>
        /// Classrooms where the course is in their program
        /// </summary>
        public List<GradesByClassRoomHelper> ClassRooms { get; set; }
    }

    /// <summary>
    /// We also want to list the students in the classroom taking the course
    /// </summary>
    public class GradesByClassRoomHelper
    {
        /// <summary>
        /// Classroom name
        /// </summary>
        public string ClassRoom { get; set; }

        /// <summary>
        /// The students list (only those students that are taking the course)
        /// </summary>
        public List<GradesByStudentHelper> Students { get; set; }
    }

    /// <summary>
    /// We need a succint structure with info about grades by a studen
    /// </summary>
    public class GradesByStudentHelper
    {
        /// <summary>
        /// The student's full name
        /// </summary>
        public string Student { get; set; }

        /// <summary>
        /// See below
        /// </summary>
        public List<GradesHelper> Grades { get; set; }
        // public double GradePointAverage { get; set; }
    }

    /// <summary>
    /// The minimal info about a grade
    /// GradesByStudentHelper is using this
    /// </summary>
    public class GradesHelper
    {
        /// <summary>
        /// The grade...
        /// </summary>
        public int Grade { get; set; }
    }
}