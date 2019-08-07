using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class TeachingNotFoundException : Exception
    {
        public TeachingNotFoundException()
        {
        }

        public TeachingNotFoundException(string message) : base(message)
        {
        }

        public TeachingNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TeachingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}