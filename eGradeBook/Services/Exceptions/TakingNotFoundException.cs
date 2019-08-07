using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class TakingNotFoundException : Exception
    {
        public TakingNotFoundException()
        {
        }

        public TakingNotFoundException(string message) : base(message)
        {
        }

        public TakingNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TakingNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}