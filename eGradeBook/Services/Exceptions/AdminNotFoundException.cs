using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class AdminNotFoundException : Exception
    {
        public AdminNotFoundException()
        {
        }

        public AdminNotFoundException(string message) : base(message)
        {
        }

        public AdminNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AdminNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}