using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// Final grade model.
    /// </summary>
    public class FinalGrade
    {
        /// <summary>
        /// Id of the final grade
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Grades are in the range from 1 to 5. No fancy stuff here.
        /// </summary>
        [Range(1, 5, ErrorMessage = "Grades are from 1 to 5")]
        public int GradePoint { get; set; }

        /// <summary>
        /// A final grade can be for the first term or the second term and has to be unique.
        /// </summary>
        [Index("IX_Taking_SchoolTerm", IsUnique = true, Order = 1)]
        [Range(1, 2, ErrorMessage = "First or second term are possible values")]
        public int SchoolTerm { get; set; }

        /// <summary>
        /// A final grade is for a course taken by a student from a given classroom and given by the teacher teaching the course in the classroom
        /// </summary>
        [Index("IX_Taking_SchoolTerm", IsUnique = true, Order = 2)]
        public int TakingId { get; set; }

        /// <summary>
        /// The teacher can leave a note about the final grade
        /// </summary>
        public string Notes { get; set; }

        public DateTime Assigned { get; set; }

        /// <summary>
        /// The created column is created by the database. It's implementation dependent, I'll maybe change it later
        /// </summary>
        [Timestamp]
        public byte[] Created { get; set; }

        /// <summary>
        /// If anything is changed, we need to update this property and leave a note (just a convention, it cannot be enforced as it is)
        /// </summary>
        public DateTime? LastChange { get; set; }

        /// <summary>
        /// The foreign key for the course + teacher + classroom + student combo
        /// </summary>
        [ForeignKey("TakingId")]
        public virtual Taking Taking { get; set; }

        // TODO The user (teacher or admin) who is giving the grade
    }
}