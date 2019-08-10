using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http.Controllers;

namespace eGradeBook.Utilities.WebApi
{
    /// <summary>
    /// Helper class for working with Identity data for logged in users of the API
    /// TODO put the converter here also
    /// </summary>
    public static class IdentityHelper
    {
        /// <summary>
        /// Fetch user identity data from the Request Context
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        public static RequestUserData FetchUserData(HttpRequestContext requestContext)
        {
            var principal = requestContext.Principal;

            var isAdmin = principal.IsInRole("admins");
            var isTeacher = principal.IsInRole("teachers");
            var isStudent = principal.IsInRole("students");
            var isParent = principal.IsInRole("parents");
            var isAuthenticated = principal.Identity.IsAuthenticated;
            var userEmail = ((ClaimsPrincipal)principal).FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            var userId = ((ClaimsPrincipal)principal).FindFirst(x => x.Type == "UserId")?.Value;

            string userRole = null;

            // TODO save role in string, if you want to add that info to log...
            if (isAdmin)
            {
                userRole = "admins";
            }
            else if (isTeacher)
            {
                userRole = "teachers";
            }
            else if (isStudent)
            {
                userRole = "students";
            }
            else if (isParent)
            {
                userRole = "parents";
            }

            
            int.TryParse(userId, out int myId);

            return new RequestUserData()
            {
                IsAdmin = isAdmin,
                IsTeacher = isTeacher,
                IsStudent = isStudent,
                IsParent = isParent,
                IsAuthenticated = isAuthenticated,
                UserId = myId,
                UserEmail = userEmail,
                UserRole = userRole
            };
        }

        /// <summary>
        /// Get Logged in user with id and role
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        public static LoggedInUser GetLoggedInUser(RequestUserData userData)
        {
            return new LoggedInUser()
            {
                UserId = userData.UserId,
                UserRole = userData.UserRole
            };
        }

        /// <summary>
        /// Get Logged in user with id and role directly from Context
        /// Used when we are not needing every information
        /// </summary>
        /// <param name="requestContext"></param>
        /// <returns></returns>
        public static LoggedInUser GetLoggedInUser(HttpRequestContext requestContext)
        {
            var userData = FetchUserData(requestContext);

            return new LoggedInUser()
            {
                UserId = userData.UserId,
                UserRole = userData.UserRole
            };
        }
    }
}