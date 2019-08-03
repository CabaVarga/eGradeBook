using eGradeBook.Models;
using eGradeBook.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Unity;

namespace eGradeBook.Providers
{
    /// <summary>
    /// Authorization server processing OAuth2 request for tokens
    /// </summary>
    public class CustomAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private UnityContainer container;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructo
        /// </summary>
        /// <param name="container"></param>
        public CustomAuthorizationServerProvider(UnityContainer container)
        {
            this.container = container;
            logger.Trace("Auth server constructed");
        }

        /// <summary>
        /// Grant resource owner credentials 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            // Cannot set in constructor -- of course I cant, for the object itself is not yet created?
            logger.Trace("IP: {0} username={1}", context.Request.RemoteIpAddress, context.UserName);

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim> user = null;
            IEnumerable<string> roles = null;

            IAuthRepository _repo = container.Resolve<IAuthRepository>();

            user = await _repo.FindUser(context.UserName, context.Password);

            if (user == null)
            {
                logger.Trace("username={0} and password combo is not valid", context.UserName);
                context.SetError("invalid_grant", "The user name or password is incorrect");
                context.Rejected();
                return;
            }

            roles = await _repo.FindRoles(user.Id);

            var ticket = new AuthenticationTicket(SetClaimsIdentity(context, (GradeBookUser)user, roles), new AuthenticationProperties());
            context.Validated(ticket);

            _repo.Dispose();
        }

        /// <summary>
        /// Validate client's authentication
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Fill the claims inside the token
        /// </summary>
        /// <param name="context"></param>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        private static ClaimsIdentity SetClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, GradeBookUser user, IEnumerable<string> roles)
        {
            // Just for reference: context.Options.AuthenticationType
            var identity = new ClaimsIdentity("JWT");
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("UserId", user.Id.ToString()));

            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return identity;
        }
    }
}