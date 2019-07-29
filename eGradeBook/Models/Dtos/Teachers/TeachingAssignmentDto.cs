using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Teachers
{
    public class TeachingAssignmentDto
    {
        [Required]
        public int TeacherId { get; set; }

        [Required]
        public int SubjectId { get; set; }
    }
}