using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Grades;
using eGradeBook.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Web api controller for working with grades
    /// </summary>
    [RoutePrefix("api/grades")]
    public class GradesController : ApiController
    {
        // private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IGradesService gradesService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gradesService"></param>
        /// <param name="log"></param>
        public GradesController(IGradesService gradesService)
        {
            this.gradesService = gradesService;
        }

        /// <summary>
        /// REST Endpoint for grade assignment
        /// The current version is using URI parameters but a special structure would be more fitting.
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="studentId"></param>
        /// <param name="subjectId"></param>
        /// <param name="gradePoint"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        [Authorize(Roles = "teachers")]
        public GradeDto PostGrade(int teacherId, int studentId, int subjectId, int gradePoint, string notes = null)
        {
            var claimsPrincipal = (ClaimsPrincipal)RequestContext.Principal;
            string userEmail = claimsPrincipal.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            string userId = claimsPrincipal.FindFirst(x => x.Type == "UserId")?.Value;

            return gradesService.CreateGrade(teacherId, studentId, subjectId, gradePoint, notes);
        }

        /// <summary>
        /// Retrieve all grades.
        /// It will first identify the user by role and id
        /// The call the matching service method and return the list of grades
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IHttpActionResult GetGrades()
        {
            bool isAdmin = RequestContext.Principal.IsInRole("admins");
            bool isTeacher = RequestContext.Principal.IsInRole("teachers");
            bool isStudent = RequestContext.Principal.IsInRole("students");
            bool isParent = RequestContext.Principal.IsInRole("parents");
            bool isClassMaster = RequestContext.Principal.IsInRole("classmasters");

            string userIdString = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId")?.Value;
            int userId = 0;

            if (userIdString != null)
            {
                userId = int.Parse(userIdString);
            }

            if (isAdmin)
            {
                return Ok(gradesService.GetAllGrades());
            }

            if (isTeacher)
            {
                return Ok(gradesService.GetAllGradesForTeacher(userId));
            }

            if (isParent)
            {
                return Ok(gradesService.GetAllGradesForParent(userId));
            }

            return Ok();

        }

        /// <summary>
        /// Created for testing purposes because the main endpoint is accessible only for authorized users
        /// </summary>
        /// <returns></returns>
        [Route("api/grades/forpublic")]
        public IHttpActionResult GetGradesForPublic()
        {
            return Ok(gradesService.GetGradesByCourses());
        }

        /// <summary>
        /// Retrieve grades by different criteria
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="gradeId"></param>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        /// <param name="semesterId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [Route("query")]
        public IHttpActionResult GetGradesByParameters(
            [FromUri]int? studentId = null, 
            [FromUri]int? gradeId = null, 
            [FromUri]int? teacherId = null, 
            [FromUri]int? courseId = null, 
            [FromUri]int? semesterId = null, 
            [FromUri]int? classId = null)
        {
            logger.Trace("Tracer, is authenticated -- {0}", this.User.Identity.IsAuthenticated);
            return Ok(gradesService.GetGradesByParameters(studentId, gradeId, teacherId, courseId, semesterId, classId));
        }

        /// <summary>
        /// A short, temporary endpoint for some testing
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [Route("query2")]
        public IHttpActionResult GetGradesByParameters2([FromUri]int? studentId)
        {
            return Ok(gradesService.GetGradesByParameters(studentId, null, null, null, null, null));
        }

    }
}
