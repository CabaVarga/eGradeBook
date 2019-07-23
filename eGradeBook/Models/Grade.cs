using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Grade
    {
        public int Id { get; set; }

        [ForeignKey("Student")]
        public int StudentId { get; set; }

        [ForeignKey("Teacher")]
        public int TeacherId { get; set; }

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }

        [Range(1, 5, ErrorMessage = "Grades are from 1 to 5")]
        public int GradePoint { get; set; }

        public string Notes { get; set; }

        public DateTime Assigned { get; set; }

        public DateTime? LastChange { get; set; }

        public virtual StudentUser Student { get; set; }

        public virtual TeacherUser Teacher { get; set; }

        public virtual Course Subject { get; set; }
    }
}