using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.ClassRooms
{
    public class ClassRoomEnrollStudentDto
    {
        public int StudentId { get; set; }
        public int ClassRoomId { get; set; }
    }
}