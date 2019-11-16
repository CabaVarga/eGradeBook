using eGradeBook.Models.Dtos.Grades;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.SwaggerHelpers.Examples
{
    public class CreateGradeByTeacherExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new GradeDto()
            {
                TakingId = 1,
                Semester = 1,
                AssignmentDate = new DateTime(2019, 09, 10),
                GradePoint = 2,
                Notes = "Arithmetics"
            };
        }
    }
}