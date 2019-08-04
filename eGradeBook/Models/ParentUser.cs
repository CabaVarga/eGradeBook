using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// Parent of a student of the school
    /// </summary>
    public class ParentUser : GradeBookUser
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ParentUser()
        {
            this.StudentParents = new HashSet<StudentParent>();
        }

        /// <summary>
        /// A gerund, association between students and parents
        /// TODO add the key to be directly accessible
        /// </summary>
        public virtual ICollection<StudentParent> StudentParents { get; set; }
    }
}