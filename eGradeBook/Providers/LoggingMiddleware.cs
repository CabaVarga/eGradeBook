﻿using Microsoft.Owin;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Providers
{
    /// <summary>
    /// Logging incoming requests and outgoing responses
    /// </summary>
    public class LoggingMiddleware : OwinMiddleware
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        public LoggingMiddleware(OwinMiddleware next) : base(next)
        {
        }

        /// <summary>
        /// Invoke method
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task Invoke(IOwinContext context)
        {
            MappedDiagnosticsLogicalContext.Set("RequestGuid", Guid.NewGuid().ToString());
            MappedDiagnosticsContext.Set("RequestGuid", Guid.NewGuid().ToString());
            GlobalDiagnosticsContext.Set("RequestGuid", Guid.NewGuid().ToString());

            //logger.Info("layer={0} ip={1} method={2} path={3} uri={4} phase={5}",
            //   "owin",
            //    context.Request.RemoteIpAddress, 
            //    context.Request.Method, 
            //    context.Request.Path,
            //    context.Request.Uri,
            //    "request");

            var stopWatch = new Stopwatch();

            var requestData = new
            {
                RemoteIP = context.Request.RemoteIpAddress,
                Method = context.Request.Method,
                Path = context.Request.Path,
                ResourceURI = context.Request.Uri
            };

            if (!context.Request.Path.ToString().Contains("swagger"))
            {
                logger.Trace("HttpRequest {requestData}", requestData);
            }

            stopWatch.Start();

            await Next.Invoke(context);

            // logger.Info("layer={0} status={1} phase={2}", "owin", context.Response.StatusCode, "response");

            var miliseconds = stopWatch.ElapsedMilliseconds;
            stopWatch.Reset();
            var responseData = new
            {
                StatusCode = context.Response.StatusCode,
                ProcessingTime = miliseconds
            };

            if (!context.Request.Path.ToString().Contains("swagger"))
            {
                logger.Trace("HttpResponse {responseData}", responseData);
            }
        }
    }
}