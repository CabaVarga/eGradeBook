using eGradeBook.Models;
using eGradeBook.Models.Dtos.Teachings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    public static class TeachingsConverter
    {
        public static TeachingDto TeachingToTeachingDto(Teaching teaching)
        {
            return new TeachingDto()
            {
                CourseId = teaching.CourseId,
                TeacherId = teaching.TeacherId
            };
        }
    }
}