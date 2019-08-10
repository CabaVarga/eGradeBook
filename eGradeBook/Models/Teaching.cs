using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// An association class between a teacher and a course
    /// </summary>
    public class Teaching
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Teacher Id
        /// NOTE the teacher + course combination must be unique
        /// </summary>
        [Index("IX_Teacher_Subject", IsUnique = true, Order = 1)]
        public int TeacherId { get; set; }

        /// <summary>
        /// Course Id
        /// </summary>
        [Index("IX_Teacher_Subject", IsUnique = true, Order = 2)]
        public int CourseId { get; set; }

        /// <summary>
        /// The teacher in the relation
        /// </summary>
        [ForeignKey("TeacherId")]
        // [Required]
        public virtual TeacherUser Teacher { get; set; }

        /// <summary>
        /// The course in the relation
        /// </summary>
        [ForeignKey("CourseId")]
        // [Required]
        public virtual Course Course { get; set; }


        public virtual ICollection<TeachingProgram> TeachingPrograms { get; set; }
    }
}