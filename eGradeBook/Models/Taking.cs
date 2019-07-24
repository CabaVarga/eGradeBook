using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Taking
    {
        public int Id { get; set; }

        [Index("IX_Course_Student", IsUnique = true, Order = 1)]
        public int CourseId { get; set; }

        [Index("IX_Course_Student", IsUnique = true, Order = 2)]
        public int StudentId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [ForeignKey("StudentId")]
        public virtual StudentUser Student { get; set; }
    }
}