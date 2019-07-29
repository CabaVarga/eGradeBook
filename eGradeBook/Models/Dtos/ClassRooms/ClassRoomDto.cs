using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.ClassRooms
{
    public class ClassRoomDto
    {
        public class ClassRoomStudentDto
        {
            public int StudentId { get; set; }
            public string FullName { get; set; }
        }

        public class ClassRoomProgramDto
        {
            public int CourseId { get; set; }
            public int TeacherId { get; set; }
            public string Course { get; set; }
            public string Teacher { get; set; }
            public int TeachingHours { get; set; }
        }

        public ClassRoomDto()
        {
            this.Students = new List<ClassRoomStudentDto>();
            this.Program = new List<ClassRoomProgramDto>();
        }

        public int ClassRoomId { get; set; }
        public string Name { get; set; }
        public int SchoolGrade { get; set; }

        public List<ClassRoomStudentDto> Students { get; set; }
        public List<ClassRoomProgramDto> Program { get; set; }
    }
}