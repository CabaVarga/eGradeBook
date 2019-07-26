using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Taking
    {
        public int Id { get; set; }

        [Index("IX_Student_Program", IsUnique = true, Order = 1)]
        public int ProgramId { get; set; }

        [Index("IX_Student_Program", IsUnique = true, Order = 2)]
        public int StudentId { get; set; }

        [Required]
        [ForeignKey("ProgramId")]
        public virtual Program Program { get; set; }

        [Required]
        [ForeignKey("StudentId")]
        public virtual StudentUser Student { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<FinalGrade> FinalGrades { get; set; }
    }
}