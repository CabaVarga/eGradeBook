using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class StudentParentNotFoundException : Exception
    {
        public StudentParentNotFoundException()
        {
        }

        public StudentParentNotFoundException(string message) : base(message)
        {
        }

        public StudentParentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StudentParentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}