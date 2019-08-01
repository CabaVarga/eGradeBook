using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace eGradeBook.Utilities.WebApi
{
    /// <summary>
    /// Helper class to log exceptions at the web api level
    /// </summary>
    public class NLogExceptionLogger : ExceptionLogger
    {
        private static readonly Logger Nlog = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The log command
        /// </summary>
        /// <param name="context"></param>
        public override void Log(ExceptionLoggerContext context)
        {
            Nlog.Log(LogLevel.Error, context.Exception, RequestToString(context.Request));
        }

        /// <summary>
        /// Helper method
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private static string RequestToString(HttpRequestMessage request)
        {
            var message = new StringBuilder();
            if (request.Method != null)
                message.Append(request.Method);

            if (request.RequestUri != null)
                message.Append(" ").Append(request.RequestUri);

            return message.ToString();
        }
    }
}