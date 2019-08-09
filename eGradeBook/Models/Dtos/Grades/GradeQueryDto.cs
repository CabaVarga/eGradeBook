using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Grades
{
    public class GradeQueryDto
    {
        public int? GradeId { get; set; } = null;
        public int? CourseId { get; set; } = null;
        public int? TeacherId { get; set; } = null;
        public int? ClassRoomId { get; set; } = null;
        public int? StudentId { get; set; } = null;
        public int? ParentId { get; set; } = null;
        public int? Semester { get; set; } = null;
        public int? SchoolGrade { get; set; } = null;
        public int? Grade { get; set; } = null;
        public DateTime? FromDate { get; set; } = null;
        public DateTime? ToDate { get; set; } = null;
    }
}