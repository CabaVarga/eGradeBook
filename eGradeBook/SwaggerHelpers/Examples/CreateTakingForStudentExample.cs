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
                EnrollmentId = 1,
                ProgramId = 1
            };
        }
    }
}