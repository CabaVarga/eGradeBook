using eGradeBook.Models;
using eGradeBook.Models.Dtos.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Converters for student users and their relations
    /// </summary>
    public static class StudentsConverter
    {
        /// <summary>
        /// Convert a student model to student dto
        /// </summary>
        /// <param name="student">A student (full) model</param>
        /// <returns>Student Dto object, ready for Json serialization</returns>
        public static StudentDto StudentToStudentDto(StudentUser student)
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