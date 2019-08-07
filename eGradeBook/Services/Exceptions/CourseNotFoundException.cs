using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class CourseNotFoundException : Exception
    {
        public CourseNotFoundException()
        {
        }

        public CourseNotFoundException(string message) : base(message)
        {
        }

        public CourseNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CourseNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}