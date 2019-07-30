using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    /// <summary>
    /// The main star, the Grade
    /// </summary>
    public class Grade
    {
        /// <summary>
        /// Id of the grade -- every grade is an individual entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The grade point
        /// </summary>
        [Range(1, 5, ErrorMessage = "Grades are from 1 to 5")]
        public int GradePoint { get; set; }

        /// <summary>
        /// The grades are being given in the first or second term
        /// </summary>
        [Range(1, 2, ErrorMessage = "First or second term are possible values")]
        public int SchoolTerm { get; set; }

        /// <summary>
        /// Often times the teacher will leave a note with the grade.
        /// In a full solution maybe we can make the notes private (for teachers only)
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// The date when the grade was assigned
        /// </summary>
        public DateTime Assigned { get; set; }

        /// <summary>
        /// The date when the grade was added to the system
        /// </summary>
        [Timestamp]
        public byte[] Created { get; set; }

        /// <summary>
        /// The last time the grade was changed
        /// </summary>
        public DateTime? LastChange { get; set; }

        /// <summary>
        /// The structure containing the student + course + classroom + teacher combo
        /// </summary>
        public virtual Taking Taking { get; set; }
    }
}