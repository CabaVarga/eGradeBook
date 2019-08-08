using eGradeBook.Models;
using eGradeBook.Models.Dtos.Takings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    public static class TakingsConverter
    {
        public static TakingDto TakingToTakingDto(Taking taking)
        {
            return new TakingDto()
            {
                TakingId = taking.Id,
                CourseId = taking.Program.Teaching.CourseId,
                TeacherId = taking.Program.Teaching.TeacherId,
                ClassRoomId = taking.Program.ClassRoomId,
                StudentId = taking.StudentId
            };
        }
    }
}