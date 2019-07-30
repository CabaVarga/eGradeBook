using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.ClassRooms
{
    /// <summary>
    /// Dto to attach a student to a classroom
    /// </summary>
    public class ClassRoomEnrollStudentDto
    {
        /// <summary>
        /// The student's Id
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// The classroom's Id
        /// </summary>
        public int ClassRoomId { get; set; }
    }
}