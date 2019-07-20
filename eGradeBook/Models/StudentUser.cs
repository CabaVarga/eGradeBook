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
            this.Parents = new HashSet<ParentUser>();
        }

        [ForeignKey("ClassRoom")]
        public int? ClassRoomId { get; set; }

        public virtual ClassRoom ClassRoom { get; set; }

        public virtual ICollection<ParentUser> Parents { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
    }
}