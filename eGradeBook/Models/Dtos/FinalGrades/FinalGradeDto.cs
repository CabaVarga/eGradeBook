using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.FinalGrades
{
    public class FinalGradeDto
    {
        public string Student { get; set; }
        public string Subject { get; set; }
        public int SchoolGrade { get; set; }
        public int Semester { get; set; }
        public int FinalGrade { get; set; }
    }
}