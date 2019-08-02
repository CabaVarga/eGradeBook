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

            logger.Info("layer={0} ip={1} method={2} path={3} uri={4} phase={5}",
                "owin",
                context.Request.RemoteIpAddress, 
                context.Request.Method, 
                context.Request.Path,
                context.Request.Uri,
                "request");

            await Next.Invoke(context);

            logger.Info("layer={0} status={1} phase={2}", "owin", context.Response.StatusCode, "response");
        }
    }
}