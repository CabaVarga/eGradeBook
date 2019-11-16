using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// A student of the school
    /// </summary>
    public class StudentUser : GradeBookUser
    {
        /// <summary>
        /// Constructor
        /// NOTE After some reading I'm not sure that this is a good practice
        /// </summary>
        public StudentUser()
        {
            this.StudentParents = new HashSet<StudentParent>();
        }

        /// <summary>
        /// The students place of birth
        /// </summary>
        public string PlaceOfBirth { get; set; }

        /// <summary>
        /// The students date of birth
        /// </summary>
        public DateTime? DateOfBirth { get; set; }

        ///// <summary>
        ///// Which classroom is the student enrolled in?
        ///// Not a Requested property because we can define a student and enroll in a class later.
        ///// Perhaps. Probably not :(
        ///// </summary>
        //public int? ClassRoomId { get; set; }

        ///// <summary>
        ///// The ClassRoom the Student is enrolled in
        ///// NOTE in a more ideal solution this would be an associative relation
        ///// </summary>
        //[ForeignKey("ClassRoomId")]
        //public virtual ClassRoom ClassRoom { get; set; }

        /// <summary>
        /// Associative class between Students and their parents
        /// NOTE Enables later improvements like a defined relation between a Guardian and a student...
        /// </summary>
        public virtual ICollection<StudentParent> StudentParents { get; set; }

        ///// <summary>
        ///// What courses does the student take?
        ///// </summary>
        //public virtual ICollection<Taking> Takings { get; set; }

        /// <summary>
        /// A student is enrolled in a classroom (one at a time)
        /// </summary>
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}