using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Grades;
using eGradeBook.Services;
using eGradeBook.SwaggerHelpers.Examples;
using eGradeBook.Utilities.WebApi;
using NLog;
using Swashbuckle.Examples;
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
    [Authorize]
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
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Grade {@gradeId} by {@userData}", gradeId, userData);

            // --- Dispatching

            var userInfo = Utilities.WebApi.IdentityHelper.FetchUserData(RequestContext);

            if (userInfo.UserId == null)
            {
                return Unauthorized();
            }

            var userId = userInfo.UserId ?? 0;

            GradeDto grade = null;

            if (userInfo.IsAdmin)
            {
                grade = gradesService.GetGradeById(gradeId);
            }
            else if (userInfo.IsTeacher)
            {
                grade = gradesService.GetGradesByParameters(gradeId: gradeId, teacherId: userData.UserId).FirstOrDefault();
            }
            else if (userInfo.IsStudent)
            {
                grade = gradesService.GetGradesByParameters(gradeId: gradeId, studentId: userData.UserId).FirstOrDefault();
            }
            else if (userInfo.IsParent)
            {
                grade = gradesService.GetGradesByParameters(gradeId: gradeId, parentId: userData.UserId).FirstOrDefault();
            }
            else
            {
                logger.Error("Authenticated user with no role --- this should not happen");
                return InternalServerError();
            }

            if (grade == null)
            {
                return NotFound();
            }

            return Ok(grade);
        }




        /// <summary>
        /// Get all grades PUBLIC --- testing
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("api/grades/forpublic")]
        public IHttpActionResult GetGradesForPublic()
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Grades by {@userData}", userData);

            logger.Info("Retrieving all grades from public endpoint");

            return Ok(gradesService.GetAllGrades());
        }

        /// <summary>
        /// Retrieve grades by different criteria
        /// </summary>
        /// <returns></returns>
        [Route("query")]
        [HttpGet]
        public IHttpActionResult GetGradesByParameters(
            [FromUri]int? gradeId = null,
            [FromUri]int? courseId = null,
            [FromUri]int? teacherId = null,
            [FromUri]int? classRoomId = null,
            [FromUri]int? studentId = null,
            [FromUri]int? parentId = null,
            [FromUri]int? semester = null,
            [FromUri]int? schoolGrade = null,
            [FromUri]int? grade = null,
            [FromUri]DateTime? fromDate = null,
            [FromUri]DateTime? toDate = null)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);            

            logger.Info("Get Grades : Query by {@userData}", studentId, userData);

            logger.Trace("Tracer, is authenticated -- {0}", this.User.Identity.IsAuthenticated);

            var grades = gradesService.GetGradesByParameters(gradeId, courseId, teacherId, classRoomId, studentId, parentId, semester, schoolGrade, grade, fromDate, toDate);

            if (grades == null)
            {
                return NotFound();
            }

            return Ok(grades);
        }

        /// <summary>
        /// Get grades by parameters, same as query but using a dto
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [Route("query-dto")]
        [Authorize(Roles = "admins")]
        [HttpGet]
        public IHttpActionResult GetGradesFromDtoQuery([FromUri]GradeQueryDto query)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Grades for query {@gradeQueryData} by {@userData}", query, userData);

            var grades = gradesService.GetGradesByParameters(query);

            if (grades == null)
            {
                return NotFound();
            }

            return Ok(grades);
        }

        [Authorize(Roles = "admins,teachers")]
        [Route("{gradeId}")]
        [HttpPut]
        public IHttpActionResult PutUpdateGrade(int gradeId, GradeDto gradeDto)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Update Grade {@gradeData} for Grade {@gradeId} by {@userData}", gradeDto, gradeId, userData);

            if (gradeId != gradeDto.GradeId)
            {
                return BadRequest("Ids do not match");
            }

            GradeDto result = gradesService.UpdateGrade(gradeDto);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        /// <summary>
        /// Create grade as teacher
        /// </summary>
        /// <param name="gradeDto"></param>
        /// <returns></returns>
        [Route("")]
        [SwaggerRequestExample(typeof(GradeDto), typeof(CreateGradeByTeacherExample))]
        [HttpPost]
        [Authorize(Roles = "admins,teachers")]
        public IHttpActionResult CreateGrade(GradeDto gradeDto)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create Grade {@gradeData} for Teacher {@teacherId} by {@userData}", gradeDto, userData);

            GradeDto createdGrade = gradesService.CreateGrade(gradeDto);

            if (createdGrade == null)
            {
                return NotFound();
            }

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
        [Route("")]
        public IHttpActionResult GetGradesAll()
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Grades by {@userData}", userData);

            logger.Info("Get grades for logged in user --- auto dispatch");

            var userInfo = Utilities.WebApi.IdentityHelper.FetchUserData(RequestContext);

            if (userInfo.UserId == null)
            {
                return Unauthorized();
            }

            var userId = userInfo.UserId ?? 0;

            if (userInfo.IsAdmin)
            {
                return Ok(gradesService.GetAllGrades());
            }
            else if (userInfo.IsTeacher)
            {
                return Ok(gradesService.GetGradesByParameters(teacherId: userId));
            }
            else if (userInfo.IsStudent)
            {
                return Ok(gradesService.GetGradesByParameters(studentId: userId));
            }
            else if (userInfo.IsParent)
            {
                return Ok(gradesService.GetGradesByParameters(parentId: userId));
            }
            else
            {
                logger.Error("Authenticated user with no role --- this should not happen");
                return InternalServerError();
            }
        }


    }
}
