using eGradeBook.Models;
using eGradeBook.Models.Dtos.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    public static class TeachersConverter
    {
        /// <summary>
        /// Convert a teacher model to teacher dto
        /// </summary>
        /// <param name="teacher">A teacher (full) model</param>
        /// <returns>Teacher Dto object, ready for Json serialization</returns>
        public static TeacherDto TeacherToTeacherDto(TeacherUser teacher)
        {
            return new TeacherDto()
            {
                TeacherId = teacher.Id,
                Name = teacher.FirstName + " " + teacher.LastName,
                Courses = teacher.Teachings?.Select(t => new TeacherDto.CourseList()
                {
                    Id = t.Course.Id,
                    Name = t.Course.Name
                }).ToList()
            };
        }
    }
}