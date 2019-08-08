using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class ProgramNotFoundException : Exception
    {
        public ProgramNotFoundException()
        {
        }

        public ProgramNotFoundException(string message) : base(message)
        {
        }

        public ProgramNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ProgramNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}