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
                ProgramId = taking.Program.Id,
                EnrollmentId = taking.EnrollmentId
            };
        }
    }
}