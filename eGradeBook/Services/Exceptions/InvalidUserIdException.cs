using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    /// <summary>
    /// Invalid User Id
    /// </summary>
    public class InvalidUserIdException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public InvalidUserIdException(string message) : base(message) { }
    }
}