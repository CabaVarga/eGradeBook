using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class StudentUser : GradeBookUser
    {
        public StudentUser()
        {
            this.StudentParents = new HashSet<StudentParent>();
        }

        public int? ClassRoomId { get; set; }

        [ForeignKey("ClassRoomId")]
        public virtual ClassRoom SchoolClass { get; set; }

        // public virtual ICollection<ParentUser> Parents { get; set; }

        public virtual ICollection<StudentParent> StudentParents { get; set; }

        public virtual ICollection<Taking> Advancements { get; set; }
    }
}