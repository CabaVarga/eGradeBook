using eGradeBook.Models.Dtos.Programs;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.SwaggerHelpers.Examples
{
    public class CreateProgramExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ProgramDto()
            {
                CourseId = 1,
                TeacherId = 3,
                ClassRoomId = 1,
                WeeklyHours = 5
            };
        }
    }
}