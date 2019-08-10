using eGradeBook.Models.Dtos.ClassRooms;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.SwaggerHelpers.Examples
{
    public class CreateClassRoomProgramExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ClassRoomProgramDto()
            {
                CourseId = 1,
                TeacherId = 3,
                ClassRoomId = 1,
                WeeklyHours = 4
            };
        }
    }
}