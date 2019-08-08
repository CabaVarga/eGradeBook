using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class ParentNotFoundException : Exception
    {
        public ParentNotFoundException()
        {
        }

        public ParentNotFoundException(string message) : base(message)
        {
        }

        public ParentNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParentNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}