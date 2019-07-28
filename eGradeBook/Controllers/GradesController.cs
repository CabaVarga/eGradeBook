using eGradeBook.Models.Dtos;
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
    [RoutePrefix("api/grades")]
    public class GradesController : ApiController
    {
        // private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private ILogger log;
        private IGradesService gradesService;

        public GradesController(IGradesService gradesService, ILogger log)
        {
            this.gradesService = gradesService;
            this.log = log;

            this.log.Log(LogLevel.Info, "Grades service created");
        }

        [Authorize(Roles = "teachers")]
        public GradeDto PostGrade(int teacherId, int studentId, int subjectId, int gradePoint, string notes = null)
        {
            var claimsPrincipal = (ClaimsPrincipal)RequestContext.Principal;
            string userEmail = claimsPrincipal.FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            string userId = claimsPrincipal.FindFirst(x => x.Type == "UserId")?.Value;

            return gradesService.CreateGrade(teacherId, studentId, subjectId, gradePoint, notes);
        }

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

        [Route("api/grades/forpublic")]
        public IHttpActionResult GetGradesForPublic()
        {
            return Ok(gradesService.GetGradesByCourses());
        }

        [Route("query")]
        public IHttpActionResult GetGradesByParameters(
            [FromUri]int? studentId = null, 
            [FromUri]int? gradeId = null, 
            [FromUri]int? teacherId = null, 
            [FromUri]int? courseId = null, 
            [FromUri]int? semesterId = null, 
            [FromUri]int? classId = null)
        {
            log.Trace("Tracer, is authenticated -- {0}", this.User.Identity.IsAuthenticated);
            return Ok(gradesService.GetGradesByParameters(studentId, gradeId, teacherId, courseId, semesterId, classId));
        }

        [Route("query2")]
        public IHttpActionResult GetGradesByParameters2([FromUri]int? studentId)
        {
            return Ok(gradesService.GetGradesByParameters(studentId, null, null, null, null, null));
        }

    }
}
