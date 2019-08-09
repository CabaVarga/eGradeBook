using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Grades
{
    public class GradeNotificationDto
    {
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string ParentEmail { get; set; }
        public string TeacherFirstName { get; set; }
        public string TeacherLastName { get; set; }
        public string GradePoint { get; set; }
        public string Course { get; set; }
        public string ClassRoom { get; set; }
        public string Assigned { get; set; }
    }
}