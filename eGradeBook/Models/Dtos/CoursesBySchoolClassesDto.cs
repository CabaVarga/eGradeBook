using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    public class CoursesBySchoolClassesDto
    {
        public string SchoolClass { get; set; }

        public ICollection<string> Courses { get; set; }
    }
}