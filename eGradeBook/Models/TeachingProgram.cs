using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class TeachingProgram
    {
        public virtual Teaching Teaching { get; set; }
        public virtual Program Program { get; set; }
    }
}