using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Takings
{
    public class TakingDto
    {
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
        public int ClassRoomId { get; set; }
        public int StudentId { get; set; }
    }
}