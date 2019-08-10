using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class ClassRoomNotFoundException : Exception
    {
        public ClassRoomNotFoundException()
        {
        }

        public ClassRoomNotFoundException(string message) : base(message)
        {
        }

        public ClassRoomNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClassRoomNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}