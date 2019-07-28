using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Teachers
{
    public class TeacherDto
    {
        public TeacherDto()
        {
            this.Courses = new List<CourseList>();
        }

        public class CourseList
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public string Name { get; set; }
        public int TeacherId { get; set; }
        public List<CourseList> Courses { get; set; }
    }
}