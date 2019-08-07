using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class TeacherNotFoundException : Exception
    {
        public TeacherNotFoundException()
        {
        }

        public TeacherNotFoundException(string message) : base(message)
        {
        }

        public TeacherNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TeacherNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}