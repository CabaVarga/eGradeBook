using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// A program is the collection of courses of a given classroom in the given schoolyear
    /// A syllable, a curriculum
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Program Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Weekly teaching hours
        /// </summary>
        public int WeeklyHours { get; set; }

        /// <summary>
        /// The associated course's Id
        /// TODO remove
        /// NOTE indexed as unique, for the same class cannot be added twice to the same classroom
        /// </summary>
        [Index("IX_Teacher_ClassRoom", IsUnique = true, Order = 1)]
        public int CourseId { get; set; }

        /// <summary>
        /// The classroom's Id
        /// </summary>
        [Index("IX_Teacher_ClassRoom", IsUnique = true, Order = 2)]
        public int ClassRoomId { get; set; }

        /// <summary>
        /// Teaching is a gerund between a course and a teacher teaching the course
        /// TODO remove direct link to course BUT! Then you will need to check the Course in the code!!!
        /// Otherwise one course can be attached multiple times ...
        /// Maybe not even a problem but not optimal!
        /// </summary>
        public int TeachingId { get; set; }

        // Reasoning:
        // If the SchoolClass + Course combo is unique, the teaching id is also unique by default (for a given SchoolClass + Course combo...)
        // The natural relation here is between the course and the schoolClass. The Teaching is here for the teacher. Why not directly the teacher though, then??
        // well, at least we can check directly if the subject is teached by him /her

        /// <summary>
        /// Course Id
        /// </summary>
        [Required]
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        /// <summary>
        /// Teaching is a gerund between a course and a teacher teaching the course
        /// </summary>
        [Required]
        [ForeignKey("TeachingId")]
        public virtual Teaching Teaching { get; set; }

        /// <summary>
        /// The classroom
        /// </summary>
        [Required]
        [ForeignKey("ClassRoomId")]
        public virtual ClassRoom ClassRoom { get; set; }

        /// <summary>
        /// Uhm this should be the Takings, not directly the Students...
        /// TODO URGENT
        /// </summary>
        public virtual ICollection<Taking> TakingStudents { get; set; }
    }
}