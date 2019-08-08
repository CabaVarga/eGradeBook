using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Teachings
{
    public class TeachingDto
    {
        public int TeachingId { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
    }
}