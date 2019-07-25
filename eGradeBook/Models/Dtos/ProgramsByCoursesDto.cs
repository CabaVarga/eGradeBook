using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    public class ProgramsByCoursesDto
    {
        public string CourseName { get; set; }
        public ICollection<string> SchoolClasses { get; set; }
    }

    public class ProgramsByCoursesDetail
    {
        public string ClassName { get; set; }
        public int Grade { get; set; }
        public int WeeklyHours { get; set; }
        public string Teacher { get; set; }
    }

}