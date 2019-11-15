using eGradeBook.Models;
using eGradeBook.Models.Dtos.Enrollments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    public static class EnrollmentsConverter
    {
        public static EnrollmentDto EnrollmentToEnrollmentDto(Enrollment enrollment)
        {
            return new EnrollmentDto()
            {
                EnrollmentId = enrollment.Id,
                StudentId = enrollment.StudentId,
                ClassRoomId = enrollment.ClassRoomId
            };
        }
    }
}