using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace eGradeBook.Services.Exceptions
{
    /// <summary>
    /// Used with user registration failures
    /// </summary>
    public class UserUpdateException : Exception
    {
        /// <summary>
        /// List of errors got from identity
        /// </summary>
        public string[] IdentityErrors { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public UserUpdateException(string[] errors)
        {
            IdentityErrors = errors;
        }

        /// <summary>
        /// A bit more advanced constructor
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="message"></param>
        public UserUpdateException(string[] errors, string message) : base(message)
        {
            IdentityErrors = errors;
        }

        /// <summary>
        /// Advanced constructor
        /// </summary>
        /// <param name="errors"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public UserUpdateException(string[] errors, string message, Exception innerException) : base(message, innerException)
        {
            IdentityErrors = errors;
        }

        /// <summary>
        /// Serialization
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UserUpdateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info != null)
            {
                info.AddValue("IdentityErrors", IdentityErrors);
            }
        }

        /// <summary>
        /// Used for serialization
        /// </summary>
        /// <param name="i"></param>
        /// <param name="c"></param>
        public override void GetObjectData(SerializationInfo i, StreamingContext c)
        {
            if (i != null)
            {
                IdentityErrors = (string[])i.GetValue("IdentityErrors", typeof(string[]));
            }
        }
    }
}