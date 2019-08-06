using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Teachers
{
    /// <summary>
    /// Returns a list of courses with the classrooms they are being taught in
    /// And also a list of classrooms with the courses the teacher teaches
    /// </summary>
    public class TeacherExtendedDto
    {
        /// <summary>
        /// The collection of courses and the classrooms they are taught in
        /// </summary>
        public class CourseClassRoomDto
        {
            public class InnerClassRoomDto
            {
                public int ClassRoomId { get; set; }
                public string ClassRoomName { get; set; }
                public int Grade { get; set; }
                public int WeeklyHours { get; set; }
            }

            public int CourseId { get; set; }
            public string CourseName { get; set; }

            public ICollection<InnerClassRoomDto> ClassRooms { get; set; }
        }

        /// <summary>
        /// The collection of classrooms where the teacher is teaching and the courses taught
        /// </summary>
        public class ClassRoomCoursesDto
        {
            public class InnerCourseDto
            {
                public int CourseId { get; set; }
                public string CourseName { get; set; }
                public int WeeklyHours { get; set; }
            }

            public int ClassRoomId { get; set; }
            public string ClassRoomName { get; set; }
            public int Grade { get; set; }

            public ICollection<InnerCourseDto> Courses { get; set; }
        }

        /// <summary>
        /// Teacher Id
        /// </summary>
        public int TeacherId { get; set; }

        /// <summary>
        /// Teachers first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Teachers last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The collection of courses and the classrooms they are taught in
        /// </summary>
        public ICollection<CourseClassRoomDto> Courses { get; set; }

        /// <summary>
        /// The collection of classrooms where the teacher is teaching and the courses taught
        /// </summary>
        public ICollection<ClassRoomCoursesDto> ClassRooms { get; set; }
    }
}