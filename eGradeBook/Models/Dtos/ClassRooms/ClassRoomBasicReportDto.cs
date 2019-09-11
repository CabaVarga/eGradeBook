using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.ClassRooms
{
    public class ClassRoomBasicReportDto
    {
        public int ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }
        public int ClassRoomGrade { get; set; }

        public IEnumerable<CourseDto> Courses { get; set; }
        public IEnumerable<StudentDto> Students { get; set; }

        public class CourseDto
        {
            public int CourseId { get; set; }
            public string CourseName { get; set; }
        }

        public class StudentDto
        {
            public int StudentId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}