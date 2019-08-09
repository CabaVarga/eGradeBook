using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Teachers
{
    public class TeacherReportDto
    {
        public int TeacherId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<CourseDto> Courses { get; set; }
        public IEnumerable<ClassRoomDto> ClassRooms { get; set; }

        public class CourseDto
        {
            public int CourseId { get; set; }
            public string CourseName { get; set; }

            public IEnumerable<ClassRoomDto> ClassRooms { get; set; }

            public class ClassRoomDto
            {
                public int ClassRoomId { get; set; }
                public string ClassRoomName { get; set; }
                public int Grade { get; set; }
                public int WeeklyHours { get; set; }
                public IEnumerable<StudentDto> Students { get; set; }
            }
        }

        public class ClassRoomDto
        {
            public int ClassRoomId { get; set; }
            public string ClassRoomName { get; set; }
            public int Grade { get; set; }

            public IEnumerable<CourseDto> Courses { get; set; }

            public class CourseDto
            {
                public int CourseId { get; set; }
                public string CourseName { get; set; }
                public int WeeklyHours { get; set; }

                public IEnumerable<StudentDto> Students { get; set; }
            }
        }

        public class StudentDto
        {
            public int StudentId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }

            public IEnumerable<GradeDto> Grades { get; set; }
            public IEnumerable<ParentDto> Parents { get; set; }

            public class GradeDto
            {
                public int GradeId { get; set; }
                public int GradePoint { get; set; }
                public DateTime? Assigned { get; set; }
                public int Semester { get; set; }
                public string Notes { get; set; }
            }

            public class ParentDto
            {
                public int ParentId { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
            }
        }
    }
}