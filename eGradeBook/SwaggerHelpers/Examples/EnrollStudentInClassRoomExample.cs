using eGradeBook.Models.Dtos.ClassRooms;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.SwaggerHelpers.Examples
{
    public class EnrollStudentInClassRoomExample : IExamplesProvider
    {
        public object GetExamples()
        {
            return new ClassRoomEnrollStudentDto()
            {
                ClassRoomId = 1,
                StudentId = 20
            };
        }
    }
}