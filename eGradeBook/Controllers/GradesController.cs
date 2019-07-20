using eGradeBook.Models.Dtos;
using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    public class GradesController : ApiController
    {
        private IGradesService gradesService;

        public GradesController(IGradesService gradesService)
        {
            this.gradesService = gradesService;
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
    }
}
