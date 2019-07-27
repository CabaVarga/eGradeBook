using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos
{
    public class GradesByCourseDto
    {
        public string Course { get; set; }
        public List<GradesByClassRoomHelper> ClassRooms { get; set; }
    }

    public class GradesByClassRoomHelper
    {
        public string ClassRoom { get; set; }
        public List<GradesByStudentHelper> Students { get; set; }
    }

    public class GradesByStudentHelper
    {
        public string Student { get; set; }
        public List<GradesHelper> Grades { get; set; }
        // public double GradePointAverage { get; set; }
    }

    public class GradesHelper
    {
        public int Grade { get; set; }
    }
}