using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Grading
    {
        public int Id { get; set; }

        // I PROBABLY NEED ONLY ONE INDEX, THIS ONE HERE: 
        // NO TWO TEACHERS CAN BE TEACHING THE SUBJECT TO THE STUDENT AT THE SAME TIME
        [Index("IX_Taking", IsUnique = true)]
        public int TakingId { get; set; }

        public int TeachingId { get; set; }

        [ForeignKey("TakingId")]
        public virtual Taking Taking { get; set; }

        [ForeignKey("TeachingId")]
        public virtual Teaching Teaching { get; set; }
    }
}