using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class TeacherUser : GradeBookUser
    {
        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}