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
            this.StudentParents = new HashSet<StudentParent>();
        }

        // public virtual ICollection<StudentUser> Children { get; set; }

        public virtual ICollection<StudentParent> StudentParents { get; set; }
    }
}