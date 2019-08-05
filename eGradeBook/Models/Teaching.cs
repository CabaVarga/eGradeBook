using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Teaching
    {
        public int Id { get; set; }

        [Index("IX_Teacher_Subject", IsUnique = true, Order = 1)]
        public int TeacherId { get; set; }

        [Index("IX_Teacher_Subject", IsUnique = true, Order = 2)]
        public int CourseId { get; set; }

        [ForeignKey("TeacherId")]
        [Required]
        public virtual TeacherUser Teacher { get; set; }

        [ForeignKey("CourseId")]
        [Required]
        public virtual Course Course { get; set; }

        public virtual ICollection<Program> Programs { get; set; }
    }
}