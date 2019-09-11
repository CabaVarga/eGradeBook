using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.ClassRooms
{
    public class ClassRoomFullReportDto
    {
        public int ClassRoomId { get; set; }
        public string ClassRoomName { get; set; }
        public int ClassRoomGrade { get; set; }

        public IEnumerable<CourseDto> Courses { get; set; }
        public IEnumerable<StudentDto> Students { get; set; }
        public IEnumerable<GradeDto> Grades { get; set; }
        public IEnumerable<FinalGradeDto> FinalGrades { get; set; }

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

        public class GradeDto
        {
            public int GradeId { get; set; }
            public int StudentId { get; set; }
            public int CourseId { get; set; }
            public int GradePoint { get; set; }
            public string Notes { get; set; }
            public DateTime AssignmentDate { get; set; }
            public int Semester { get; set; }


        }

        public class FinalGradeDto
        {
            public int FinalGradeId { get; set; }
            public int StudentId { get; set; }
            public int CourseId { get; set; }
            public int FinalGrade { get; set; }
            public int Semester { get; set; }
        }


    }
}