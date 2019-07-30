using eGradeBook.Models;
using eGradeBook.Models.Dtos.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    public static class StudentsConverter
    {
        /// <summary>
        /// Convert a student model to student dto
        /// </summary>
        /// <param name="student">A student (full) model</param>
        /// <returns>Student Dto object, ready for Json serialization</returns>
        public static StudentDto TeacherToTeacherDto(StudentUser student)
        {
            return new StudentDto()
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                ClassRoom = student.SchoolClass.Name,
                ClassRoomId = student.ClassRoomId
            };
        }
    }
}