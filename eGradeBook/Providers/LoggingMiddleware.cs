using Microsoft.Owin;
using NLog;
using System;
using System.Collections.Generic;
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

        public LoggingMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {

            logger.Info("Request from ip={0} method={1} path={2} uri={3} pathbase={4}", 
                context.Request.RemoteIpAddress, 
                context.Request.Method, 
                context.Request.Path,
                context.Request.Uri,
                context.Request.PathBase);

            await Next.Invoke(context);

            logger.Info("Response status={0}", context.Response.StatusCode);
        }
    }
}