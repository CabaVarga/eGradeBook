using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    public class TeachingsByCoursesDto
    {
        public string Course { get; set; }
        public ICollection<string> Teachers { get; set; }
    }
}