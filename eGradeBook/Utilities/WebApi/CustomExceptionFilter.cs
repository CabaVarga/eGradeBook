using eGradeBook.Services.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
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
    /// NOTE There are more levels: Handler level and Owin Middleware levels, + optional App level, Host level
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

            #region SqlException

            else if (exceptionType == typeof(SqlException))
            {
                var ex = actionExecutedContext.Exception as SqlException;
                
                // TODO unwrap / write out the exact errors from the errors property...

                logger.Info("Exception is of type SqlException");

                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Internal Server Error. SQL Server Error...."),
                    ReasonPhrase = "ItemNotFound"
                };

                throw new HttpResponseException(resp);
            }

            #endregion

            #region EntityFramework Exceptions

            // One group with base class EntityException https://docs.microsoft.com/en-us/dotnet/api/system.data.entityexception?view=netframework-4.8
            // https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.infrastructure?view=entity-framework-6.2.0
            // https://docs.microsoft.com/en-us/dotnet/api/system.data.dataexception?view=netframework-4.8
            // https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.core.entityexception?view=entity-framework-6.2.0
            // DbUpdateException https://docs.microsoft.com/en-us/dotnet/api/system.data.entity.infrastructure.dbupdateexception?view=entity-framework-6.2.0

            else if (exceptionType == typeof(EntityException))
            {
                logger.Info("Exception is of type EntityException");

                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent("Internal Server Error. Entity Framework Exception...."),
                    ReasonPhrase = "ItemNotFound"
                };

                throw new HttpResponseException(resp);
            }

            else if (exceptionType == typeof(DbUpdateException))
            {
                logger.Info("Exception is of type DbUpdateException");

                throw new HttpResponseException(HttpStatusCode.ExpectationFailed);
            }

            #endregion

            else if (exceptionType == typeof(UserRegistrationException))
            {
                logger.Info("Exception is of type UserRegistrationException");

                var ex = actionExecutedContext.Exception as UserRegistrationException;

                var resp = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent(String.Format("User registration failed for {0}",
                    // Will throw if IdentityErrors is empty...
                    // Better to use Data...
                        ex.IdentityErrors.Aggregate((i, j) => i + "," + j))),
                    ReasonPhrase = "Conflict"
                };

                throw new HttpResponseException(resp);
            }

            else if (exceptionType == typeof(TeacherNotFoundException))
            {
                var ex = actionExecutedContext.Exception as TeacherNotFoundException;

                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Teacher with Id {0} not found", ex.Data["teacherId"])),
                    ReasonPhrase = "Not Found"
                };

                throw new HttpResponseException(resp);
            }

            else if (exceptionType == typeof(CourseNotFoundException))
            {
                var ex = actionExecutedContext.Exception as CourseNotFoundException;

                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Course with Id {0} not found", ex.Data["courseId"])),
                    ReasonPhrase = "Not Found"
                };

                throw new HttpResponseException(resp);
            }

            else if (exceptionType == typeof(TeachingNotFoundException))
            {
                var ex = actionExecutedContext.Exception as TeachingNotFoundException;

                var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Teacher with Id {0} is not teaching the course {1}", ex.Data["teacherId"], ex.Data["courseId"])),
                    ReasonPhrase = "Not Found"
                };

                throw new HttpResponseException(resp);
            }

            else
            {
                message = "Internal server error";
                status = HttpStatusCode.InternalServerError;
            }

            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = status
            };

            base.OnException(actionExecutedContext);
        }

        #region Async if writing to database etc. I'm not using it atm
        ///// <summary>
        ///// Asynchonous exception processing
        ///// </summary>
        ///// <param name="actionExecutedContext"></param>
        ///// <param name="cancellationToken"></param>
        ///// <returns></returns>
        //public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        //{
        //    logger.Info("ASYNC On exception entered.");
        //    HttpStatusCode status = HttpStatusCode.InternalServerError;

        //    String message = String.Empty;

        //    var exceptionType = actionExecutedContext.Exception.GetType();

        //    if (exceptionType == typeof(UnauthorizedAccessException))
        //    {
        //        logger.Info("ASYNC: Exception is of type UnauthorizedAccessException");

        //        message = "Access to the Web API is not authorized.";
        //        status = HttpStatusCode.Unauthorized;
        //    }
        //    else if (exceptionType == typeof(DivideByZeroException))
        //    {
        //        message = "Internal Server Error. Division by zero happened....";
        //        status = HttpStatusCode.InternalServerError;
        //    }
        //    else
        //    {
        //        logger.Info("ASYNC: Generic exception...");

        //        message = "Not found.";
        //        status = HttpStatusCode.NotFound;
        //    }

        //    actionExecutedContext.Response = new HttpResponseMessage()
        //    {
        //        Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain"),
        //        StatusCode = status
        //    };

        //    return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        //}
        #endregion
    }
}