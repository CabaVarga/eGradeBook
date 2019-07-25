using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class FinalGrade
    {
        public int Id { get; set; }

        [Range(1, 5, ErrorMessage = "Grades are from 1 to 5")]
        public int GradePoint { get; set; }

        [Index("IX_Advancement_SchoolTerm", IsUnique = true, Order = 1)]
        [Range(1, 2, ErrorMessage = "First or second term are possible values")]
        public int SchoolTerm { get; set; }

        [Index("IX_Advancement_SchoolTerm", IsUnique = true, Order = 2)]
        public int AdvancementId { get; set; }

        public string Notes { get; set; }

        public DateTime Assigned { get; set; }

        [Timestamp]
        public DateTime Created { get; set; }

        public DateTime? LastChange { get; set; }

        [ForeignKey("AdvancementId")]
        public virtual Taking Grading { get; set; }
    }
}