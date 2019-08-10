using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Enrollment
    {
        public virtual StudentUser Student { get; set; }
        public virtual ClassRoom ClassRoom { get; set; }
    }
}