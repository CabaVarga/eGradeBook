using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Index("IX_Course_Grade", IsUnique = true, Order = 1)]
        [Required]
        [Range(1, 8, ErrorMessage = "Subjects are for grades 1 - 8")]
        public int ClassGrade { get; set; }

        [Index("IX_Course_Grade", IsUnique = true, Order = 2)]
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public string ColloqialName { get; set; }

        public int HoursPerWeek { get; set; }

        public virtual ICollection<Taking> Takings { get; set; }

        public virtual ICollection<Teaching> Teachings { get; set; }
    }
}