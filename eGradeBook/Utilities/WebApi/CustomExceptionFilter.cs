﻿using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;

namespace eGradeBook.Utilities.WebApi
{
    /// <summary>
    /// Web api exception processing
    /// Register globally and process exceptions before sending the response back
    /// </summary>
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// On exception method
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            logger.Info("On exception entered.");
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            String message = String.Empty;

            var exceptionType = actionExecutedContext.Exception.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                logger.Info("Exception is of type UnauthorizedAccessException");

                message = "Access to the Web API is not authorized.";
                status = HttpStatusCode.Unauthorized;
            }

            else if (exceptionType == typeof(DivideByZeroException))
            {
                logger.Info("Exception is of type DivideByZeroException");

                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Internal Server Error. Division by zero happened...."),
                    ReasonPhrase = "ItemNotFound"
                };

                throw new HttpResponseException(resp);
            }
            else
            {
                message = "Not found.";
                status = HttpStatusCode.NotFound;
            }

            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = status
            };

            base.OnException(actionExecutedContext);

        }

        /// <summary>
        /// Asynchonous exception processing
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            logger.Info("ASYNC On exception entered.");
            HttpStatusCode status = HttpStatusCode.InternalServerError;

            String message = String.Empty;

            var exceptionType = actionExecutedContext.Exception.GetType();

            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                logger.Info("ASYNC: Exception is of type UnauthorizedAccessException");

                message = "Access to the Web API is not authorized.";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(DivideByZeroException))
            {
                message = "Internal Server Error. Division by zero happened....";
                status = HttpStatusCode.InternalServerError;
            }
            else
            {
                logger.Info("ASYNC: Generic exception...");

                message = "Not found.";
                status = HttpStatusCode.NotFound;
            }

            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = status
            };

            return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }
    }
}