using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Teachers
{
    public class TeacherDto
    {
        public string Name { get; set; }
        public int TeacherId { get; set; }
        public List<string> Courses { get; set; }

    }
}