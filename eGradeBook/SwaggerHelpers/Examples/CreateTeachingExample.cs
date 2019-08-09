using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Models.Dtos.Teachings;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.SwaggerHelpers.Examples
{
    public class CreateTeachingExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new TeachingDto()
            {
                CourseId = 1,
                TeacherId = 2
            };
        }
    }
}