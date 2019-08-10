using eGradeBook.Models.Dtos.Takings;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.SwaggerHelpers.Examples
{
    public class CreateTakingForStudentExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new TakingDto()
            {
                CourseId = 1,
                TeacherId = 3,
                ClassRoomId = 1,
                StudentId = 20
            };
        }
    }
}