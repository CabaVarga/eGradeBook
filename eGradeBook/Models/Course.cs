using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace eGradeBook.Models
{
    public class Course
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 8, ErrorMessage = "Subjects are for grades 1 - 8")]
        public int ClassGrade { get; set; }

        public string Name { get; set; }

        public string LongName { get; set; }

        public string ShortName { get; set; }
    }
}