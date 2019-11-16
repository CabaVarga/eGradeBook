using eGradeBook.Models.Dtos.Takings;
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
using System.Web.Http;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Takings controller
    /// </summary>
    [RoutePrefix("api/takings")]
    [Authorize]
    public class TakingsController : ApiController
    {
        private ITakingsService takingsService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="takingsService"></param>
        public TakingsController(ITakingsService takingsService)
        {
            this.takingsService = takingsService;
        }

        /// <summary>
        /// Get taking by Id
        /// </summary>
        /// <param name="takingId"></param>
        /// <returns></returns>
        [Route("{takingId}", Name = "GetTaking")]
        [HttpGet]
        public IHttpActionResult GetTaking(int takingId)
        {
            logger.Info("Get taking by Id {@takingId}", takingId);

            return Ok(takingsService.GetTakingById(takingId));
        }

        /// <summary>
        /// Get all takings
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllTakingsDtos()
        {
            logger.Info("Get all takings");

            return Ok(takingsService.GetAllTakings());
        }

        // NOTE no update (at the moment. If a taking has a startDate and endDate, then yes.

        /// <summary>
        /// Create new taking
        /// </summary>
        /// <param name="takingDto"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [SwaggerRequestExample(typeof(TakingDto), typeof(CreateTakingForStudentExample))]
        public IHttpActionResult PostCreateTaking(TakingDto takingDto)
        {
            logger.Info("Create taking {@takingData}", takingDto);

            TakingDto taking = takingsService.CreateTakingDto(takingDto);

            return CreatedAtRoute("GetTaking", new { takingId = taking.TakingId }, taking);
        }

        #region QUERY
        /// <summary>
        /// Get takings based on query
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="studentId"></param>
        /// <param name="parentId"></param>
        /// <param name="courseId"></param>
        /// <param name="classRoomId"></param>
        /// <param name="schoolGrade"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("query")]
        public IHttpActionResult GetTakingsByQuery(
            [FromUri]int? teacherId = null,
            [FromUri]int? studentId = null,
            [FromUri]int? parentId = null,
            [FromUri]int? courseId = null,
            [FromUri]int? classRoomId = null,
            [FromUri]int? schoolGrade = null)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Teachers by {@userData}", userData);

            var userInfo = Utilities.WebApi.IdentityHelper.FetchUserData(RequestContext);

            if (userInfo.UserId == null)
            {
                return Unauthorized();
            }

            var userId = userInfo.UserId ?? 0;

            if (userInfo.IsAdmin)
            {
                var results = takingsService.GetTakingsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            else if (userInfo.IsTeacher)
            {
                if (teacherId != userInfo.UserId)
                {
                    throw new UnauthorizedAccessException();
                }

                var results = takingsService.GetTakingsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            else if (userInfo.IsStudent)
            {
                if (studentId != userInfo.UserId)
                {
                    throw new UnauthorizedAccessException();
                }

                var results = takingsService.GetTakingsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            else if (userInfo.IsParent)
            {
                if (parentId != userInfo.UserId)
                {
                    throw new UnauthorizedAccessException();
                }

                var results = takingsService.GetTakingsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            else
            {
                logger.Error("Authenticated user with no role --- this should not happen");
                return InternalServerError();
            }
        }
        #endregion

        /// <summary>
        /// Create new taking
        /// </summary>
        /// <param name="takingDto"></param>
        /// <returns></returns>
        [Route("{takingId}")]
        [HttpPut]
        public IHttpActionResult PutUpdateTaking(int takingId, TakingDto takingDto)
        {
            logger.Info("Update taking {@takingData}", takingDto);

            return NotFound();
        }

        /// <summary>
        /// Create new taking
        /// </summary>
        /// <param name="takingDto"></param>
        /// <returns></returns>
        [Route("{takingId}")]
        [HttpDelete]
        public IHttpActionResult DeleteTaking(int takingId)
        {
            logger.Info("Delete taking {@takingId}", takingId);

            TakingDto taking = takingsService.DeleteTakingById(takingId);

            if (taking == null)
            {
                return NotFound();
            }

            return Ok(taking);
        }
    }
}
