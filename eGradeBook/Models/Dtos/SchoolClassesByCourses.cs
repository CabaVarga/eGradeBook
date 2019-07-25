using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    public class SchoolClassesByCourses
    {
        public string Course { get; set; }
        // This is same as programs by .. only without the details....
        public ICollection<string> SchoolClasses { get; set; }
    }
}