using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class TeacherUser : GradeBookUser
    {
        public string Title { get; set; }
        public string Degree { get; set; }
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}