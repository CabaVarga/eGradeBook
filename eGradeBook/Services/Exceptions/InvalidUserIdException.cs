using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    public class InvalidUserIdException : Exception
    {
        public InvalidUserIdException(string message) : base(message) { }
    }
}