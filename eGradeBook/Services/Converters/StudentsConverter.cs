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
                PlaceOfBirth = student.PlaceOfBirth,
                DateOfBirth = student.DateOfBirth,
                ClassRoom = student.ClassRoom?.Name,
                ClassRoomId = student.ClassRoomId
            };
        }

        /// <summary>
        /// Convert a student model to student with parents dto
        /// </summary>
        /// <param name="student">A student (full) model</param>
        /// <returns>Student Dto object, ready for Json serialization</returns>
        public static StudentWithParentsDto StudentToStudentWithParentsDto(StudentUser student)
        {
            return new StudentWithParentsDto()
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                ClassRoom = student.ClassRoom.Name,
                ClassRoomId = student.ClassRoomId,
                Parents = student.StudentParents.Select(sp => new ParentsDto()
                {
                    FirstName = sp.Parent.FirstName,
                    LastName = sp.Parent.LastName,
                    ParentId = sp.ParentId
                })
            };
        }
    }
}