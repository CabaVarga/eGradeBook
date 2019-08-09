using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Students
{
    public class StudentReportDto
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PlaceOfBirth { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public ClassRoomDto ClassRoom { get; set; }
        public IEnumerable<CoursesDto> Courses { get; set; }
        public IEnumerable<TeacherDto> Teachers { get; set; }
        public IEnumerable<GradeDto> Grades { get; set; }
        public IEnumerable<ParentDto> Parents { get; set; } 


        public class ClassRoomDto
        {
            public int? ClassRoomId { get; set; }
            public string Name { get; set; }
            public int? SchoolGrade { get; set; }
        }

        public class TeacherDto
        {
            public int TeacherId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public IEnumerable<CourseDto> Courses { get; set; }

            public class CourseDto
            {
                public int CourseId { get; set; }
                public string CourseName { get; set; }
            }
        }

        public class CoursesDto
        {
            public int CourseId { get; set; }
            public string CourseName { get; set; }
            public TeacherDto Teacher { get; set; }

            public class TeacherDto
            {
                public int TeacherId { get; set; }
                public string FirstName { get; set; }
                public string LastName { get; set; }
            }
        }

        public class GradeDto
        {
            public int GradeId { get; set; }
            public int CourseId { get; set; }
            public int TeacherId { get; set; }
            public int ClassRoomId { get; set; }
            public int StudentId { get; set; }
            public int SchoolTerm { get; set; }
            public DateTime AssignmentDate { get; set; }
            public string Notes { get; set; }
            public int GradePoint { get; set; }
        }

        public class ParentDto
        {
            public int ParentId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}