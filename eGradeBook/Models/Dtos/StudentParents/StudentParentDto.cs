using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.StudentParents
{
    public class StudentParentDto
    {
        public int StudentParentId { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int ParentId { get; set; }

        public string StudentName { get; set; }

        public string ParentName { get; set; }
    }
}