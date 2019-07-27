using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Students
{
    public class StudentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ClassRoom { get; set;}
        public int StudentId { get; set; }
        public int? ClassRoomId { get; set; }
    }
}