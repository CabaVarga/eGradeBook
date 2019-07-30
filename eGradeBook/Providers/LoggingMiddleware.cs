using Microsoft.Owin;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Providers
{
    public class LoggingMiddleware : OwinMiddleware
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public LoggingMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            logger.Info("Request received in middleware...");

            await Next.Invoke(context);

            logger.Info("Response is going out...");
        }
    }
}