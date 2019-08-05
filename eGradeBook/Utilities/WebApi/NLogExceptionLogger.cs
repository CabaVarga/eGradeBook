using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            var ex = context.Exception;
            string enumerate = String.Empty;
            foreach (DictionaryEntry d in ex.Data)
            {
                Debug.WriteLine("Key" + d.Key);
                Debug.WriteLine("Value" + d.Value);
                // var type = ex.Data[d].GetType();

                // var typeIsEnum = type as IEnumerable<string>;

                //var valueSave = d.Value.GetType();


                //var typeIsStringArray = d.Value as System.String[];
             
                //if (true) // valueSave is IEnumerable<string>)
                //{
                //    foreach (var el in typeIsStringArray)
                //    {
                //        enumerate += el + " | ";
                //    }
                //    Debug.WriteLine(enumerate);
                //    enumerate = string.Empty;
                //}
            }

            var exData = new
            {
                TargetSite = ex.TargetSite,
                Message = ex.Message,
                Source = ex.Source
            };

            Nlog.Log(LogLevel.Error, context.Exception, RequestToString(context.Request) + "{exData}", exData);
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