using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Teachers
{
    public class TeacherExtendedEvenMoreDto
    {
        public class CourseClassRoomDto
        {
            public class InnerClassRoomDto
            {
                public class StudentDto
                {
                    public int StudentId { get; set; }
                    public string FirstName { get; set; }
                    public string LastName { get; set; }
                }

                public int ClassRoomId { get; set; }
                public string ClassRoomName { get; set; }
                public int Grade { get; set; }
                public int WeeklyHours { get; set; }
                public IEnumerable<StudentDto> Students { get; set; }
            }

            public int CourseId { get; set; }
            public string CourseName { get; set; }

            public ICollection<InnerClassRoomDto> ClassRooms { get; set; }
        }

        public class ClassRoomCoursesDto
        {
            public class InnerCourseDto
            {
                public class StudentDto
                {
                    public int StudentId { get; set; }
                    public string FirstName { get; set; }
                    public string LastName { get; set; }
                }

                public int CourseId { get; set; }
                public string CourseName { get; set; }
                public int WeeklyHours { get; set; }

                public IEnumerable<StudentDto> Students { get; set; }
            }

            public int ClassRoomId { get; set; }
            public string ClassRoomName { get; set; }
            public int Grade { get; set; }

            public ICollection<InnerCourseDto> Courses { get; set; }
        }

        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<CourseClassRoomDto> Courses { get; set; }
        public ICollection<ClassRoomCoursesDto> ClassRooms { get; set; }
    }
}