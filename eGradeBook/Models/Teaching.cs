using System;
using System.Collections.Generic;
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
        public int SubjectId { get; set; }

        [ForeignKey("TeacherId")]
        public virtual TeacherUser Teacher { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Course Subject { get; set; }

        public virtual ICollection<Grading> Gradings { get; set; }
    }
}