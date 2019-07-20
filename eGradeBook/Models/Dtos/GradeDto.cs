using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    public class GradeDto
    {
        public string Subject { get; set; }
        public int GradePoint { get; set; }
        public string StudentName { get; set; }
        public string TeacherName { get; set; }
    }
}