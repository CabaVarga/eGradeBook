using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    public class ProgramsBySchoolClassesDto
    {
        public string SchoolClass { get; set; }
        public ICollection<string> Courses { get; set; }
    }

    public class ProgramsBySchoolClassesDetail
    {
            public string CourseName { get; set; }
            public int Grade { get; set; }
            public int WeeklyHours { get; set; }
            public string Teacher { get; set; }
    }
}