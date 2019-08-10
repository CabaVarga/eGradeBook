using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class StudentEnrolledInCourseException : Exception
    {
        public StudentEnrolledInCourseException()
        {
        }

        public StudentEnrolledInCourseException(string message) : base(message)
        {
        }

        public StudentEnrolledInCourseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StudentEnrolledInCourseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}