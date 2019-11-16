using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Models.Dtos.Teachings;
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
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Web api for teaching assignments
    /// NOTE: superfluous (obsolete). Functionality will be in api/teachers, api/courses
    /// NOTE: maybe not
    /// </summary>
    [RoutePrefix("api/teachings")]
    [Authorize]
    public class TeachingsController : ApiController
    {
        private ITeachingsService teachings;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="teachings"></param>
        public TeachingsController(ITeachingsService teachings)
        {
            this.teachings = teachings;
        }

        /// <summary>
        /// Create teaching assignment
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        [Route("")]
        [SwaggerRequestExample(typeof(TeachingDto), typeof(CreateTeachingExample))]
        [HttpPost]
        public IHttpActionResult CreateTeachingAssignment(TeachingDto teachingDto)
        {
            logger.Info("Create teaching assignment {@teachingData}", teachingDto);

            var teaching = teachings.CreateTeaching(teachingDto);

            return CreatedAtRoute("GetTeaching", new { teachingId = teaching.TeachingId }, teaching);
        }

        /// <summary>
        /// Get all teachings
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<TeachingDto>))]
        public IHttpActionResult GetTeachings()
        {
            // let us try making it queryable...
            logger.Info("Get all teaching");
            return Ok(teachings.GetAllTeachings());
        }

        /// <summary>
        /// Get teaching by Id --- throws if not found
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        [Route("{teachingId}", Name = "GetTeaching")]
        [HttpGet]
        [ResponseType(typeof(TeachingDto))]
        public IHttpActionResult GetTeachingById(int teachingId)
        {
            logger.Info("Get teaching by Id {@teachingId}", teachingId);

            return Ok(teachings.GetTeachingById(teachingId));
        }


        // THE API IS SHIT
        // INSTEAD OF THESE STUPID BY TEACHER ETC METHODS I NEED A CLEAR
        // QUERY BASED METHOD
        // TEACHINGS -> RETURNS EITHER A TEACHING OR AN ARRAY OF TEACHINGS. CLEAR & SIMPLE


        /// <summary>
        /// Delete Teaching by teachingId
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        [Route("{teachingId}")]
        [HttpDelete]
        public IHttpActionResult DeleteTeaching(int teachingId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Trace("Delete Teaching {@teachingId} by {@userData}", teachingId, userData);

            var result = teachings.DeleteTeaching(teachingId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        #region QUERY
        /// <summary>
        /// Get teachings based on query
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
        public IHttpActionResult GetTeachingsByQuery(
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
                var results = teachings.GetTeachingsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = teachings.GetTeachingsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = teachings.GetTeachingsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = teachings.GetTeachingsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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
    }
}
