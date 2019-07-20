using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class ParentUser : GradeBookUser
    {
        public ParentUser()
        {
            this.Children = new HashSet<StudentUser>();
        }

        public virtual ICollection<StudentUser> Children { get; set; }
    }
}