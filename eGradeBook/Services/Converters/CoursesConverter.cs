using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Converter for Course entities
    /// </summary>
    public static class CoursesConverter
    {
        /// <summary>
        /// Convert a course entity to course dto
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public static CourseDto CourseToCourseDto(Course course)
        {
            return new CourseDto()
            {
                CourseId = course.Id,
                Name = course.Name,
                ColloqialName = course.ColloqialName
            };
        }
    }
}