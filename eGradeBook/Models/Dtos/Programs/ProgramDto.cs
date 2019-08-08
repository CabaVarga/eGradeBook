using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Programs
{
    public class ProgramDto
    {
        public int ProgramId { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
        public int ClassRoomId { get; set; }
        public int WeeklyHours { get; set; }
    }
}