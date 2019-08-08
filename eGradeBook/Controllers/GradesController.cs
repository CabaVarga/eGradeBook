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
        public GradesController(IGradesService gradesService)
        {
            this.gradesService = gradesService;
        }

        /// <summary>
        /// Get grade by Id
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        [Route("{gradeId}", Name = "GetGrade")]
        [HttpGet]
        public IHttpActionResult GetGradeById(int gradeId)
        {
            // TODO authorization checks..
            logger.Info("Get grade {@gradeId}", gradeId);

            var grade = gradesService.GetGradeDtoById(gradeId);

            return Ok(grade);
        }

        /// <summary>
        /// Create grade as teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="gradeDto"></param>
        /// <returns></returns>
        [Route("for-teachers/{teacherId}")]
        [HttpPost]
        public IHttpActionResult CreateGradeAsTeacher(int teacherId, GradeDto gradeDto)
        {
            logger.Info("Create grade by teacher {@teacherId} {@gradeData}", teacherId, gradeDto);
            
            if (teacherId != gradeDto.TeacherId)
            {
                return BadRequest("Id mismatch");
            }

            var userData = Utilities.WebApi.IdentityHelper.GetLoggedInUser(RequestContext);

            if (userData.UserRole != "teachers")
            {
                return Unauthorized();
            }

            if (teacherId != userData.UserId)
            {
                throw new UnauthorizedAccessException(string.Format("You are not allowed to assign grades for teacher {0}", teacherId));
            }

            GradeDto createdGrade = gradesService.CreateGradeDto(gradeDto);

            logger.Info("Teacher {@userData} created grade {@gradeData}", userData, createdGrade);

            return CreatedAtRoute("GetGrade", new { gradeId = createdGrade.GradeId }, createdGrade);            
        }

        /// <summary>
        /// Retrieve all grades.
        /// It will first identify the user by role and id
        /// The call the matching service method and return the list of grades
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("auto")]
        public IHttpActionResult GetGrades()
        {
            logger.Info("Get grades for logged in user --- auto dispatch");

            var userData = Utilities.WebApi.IdentityHelper.FetchUserData(RequestContext);

            if (userData.UserId == null)
            {
                return Unauthorized();
            }

            var userId = userData.UserId ?? 0;

            if (userData.IsAdmin)
            {
                return Ok(gradesService.GetAllGrades());
            }
            else if (userData.IsTeacher)
            {
                return Ok(gradesService.GetAllGradesForTeacher(userId));
            }
            else if (userData.IsStudent)
            {
                return Ok(gradesService.GetAllGradesForStudent(userId));
            }
            else if (userData.IsParent)
            {
                return Ok(gradesService.GetAllGradesForParent(userId));
            }
            else
            {
                logger.Error("Authenticated user with no role --- this should not happen");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Get all grades PUBLIC --- testing
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/grades-by-courses/forpublic")]
        public IHttpActionResult GetGradesForPublicByCourses()
        {
            logger.Info("Retrieving all grades from public endpoint");

            return Ok(gradesService.GetGradesByCourses());
        }

        /// <summary>
        /// Get all grades PUBLIC --- testing
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/grades/forpublic")]
        public IHttpActionResult GetGradesForPublic()
        {
            logger.Info("Retrieving all grades from public endpoint");

            return Ok(gradesService.GetAllGrades());
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
        /// NOTE testing how to retrieve all grades for student
        /// same for teacher etc...
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
