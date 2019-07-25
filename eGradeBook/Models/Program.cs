using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Program
    {
        public int Id { get; set; }

        public int WeeklyFund { get; set; }

        [Index("IX_Teacher_SchoolClass", IsUnique = true, Order = 1)]
        public int CourseId { get; set; }

        [Index("IX_Teacher_SchoolClass", IsUnique = true, Order = 2)]
        public int SchoolClassId { get; set; }

        public int TeachingId { get; set; }

        // Reasoning:
        // If the SchoolClass + Course combo is unique, the teaching id is also unique by default (for a given SchoolClass + Course combo...)
        // The natural relation here is between the course and the schoolClass. The Teaching is here for the teacher. Why not directly the teacher though, then??
        // well, at least we can check directly if the subject is teached by him /her

        [Required]
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [Required]
        [ForeignKey("TeachingId")]
        public virtual Teaching Teaching { get; set; }

        [Required]
        [ForeignKey("SchoolClassId")]
        public virtual SchoolClass SchoolClass { get; set; }

        public ICollection<StudentUser> Students { get; set; }
    }
}