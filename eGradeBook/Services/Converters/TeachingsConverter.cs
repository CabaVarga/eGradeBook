using eGradeBook.Models;
using eGradeBook.Models.Dtos.Teachings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Teachings converter
    /// </summary>
    public static class TeachingsConverter
    {
        /// <summary>
        /// Teaching to teachingDto
        /// </summary>
        /// <param name="teaching"></param>
        /// <returns></returns>
        public static TeachingDto TeachingToTeachingDto(Teaching teaching)
        {
            return new TeachingDto()
            {
                TeachingId = teaching.Id,
                CourseId = teaching.CourseId,
                TeacherId = teaching.TeacherId,
                CourseName = teaching.Course.Name,
                TeacherName = teaching.Teacher.UserName
            };
        }
    }
}