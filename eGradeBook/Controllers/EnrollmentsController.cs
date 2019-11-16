using eGradeBook.Models.Dtos.Enrollments;
using eGradeBook.Services;
using eGradeBook.Utilities.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    // NEED:
    // (1) actions
    // (2) service interface and implementation (definitions done, implementation remains)
    // (3) repo interface and implementation (DONE)
    // (4) dto conversion
    // (5) hook up with unity (DI) (DONE)
    [RoutePrefix("api/enrolments")]
    [Authorize]
    public class EnrollmentsController : ApiController
    {
        private IEnrollmentsService service;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public EnrollmentsController(IEnrollmentsService service)
        {
            this.service = service;
        }

        [Route("")]
        [HttpPost]
        [ResponseType(typeof(EnrollmentDto))]
        public IHttpActionResult CreateEnrollment(EnrollmentDto enrollment)
        {
            var result = service.CreateEnrollment(enrollment);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Route("{enrollmentId")]
        [HttpGet]
        [ResponseType(typeof(EnrollmentDto))]
        public IHttpActionResult GetEnrollmentById(int enrollmentId)
        {
            var result = service.GetEnrollmentById(enrollmentId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Route("")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<EnrollmentDto>))]
        public IHttpActionResult GetEnrollments( )
        {
            var result = service.GetEnrollments();

            return Ok(result);
        }

        [Route("{enrollmentId")]
        [HttpPut]
        [ResponseType(typeof(EnrollmentDto))]
        public IHttpActionResult UpdateEnrollment(int enrollmentId, EnrollmentDto enrollment)
        {
            if (enrollmentId != enrollment.EnrollmentId)
            {
                return BadRequest("Ids do not match");
            }

            var result = service.UpdateEnrollment(enrollmentId, enrollment);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [Route("{enrollmentId}")]
        [HttpDelete]
        [ResponseType(typeof(EnrollmentDto))]
        public IHttpActionResult DeleteEnrollment(int enrollmentId)
        {
            var result = service.DeleteEnrollment(enrollmentId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("query")]
        public IHttpActionResult GetEnrollmentsByQuery(
            [FromUri]int? teacherId = null,
            [FromUri]int? studentId = null,
            [FromUri]int? parentId = null,
            [FromUri]int? courseId = null,
            [FromUri]int? classRoomId = null,
            [FromUri]int? schoolGrade = null)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Enrollments by {@userData}", userData);

            var userInfo = Utilities.WebApi.IdentityHelper.FetchUserData(RequestContext);

            if (userInfo.UserId == null)
            {
                return Unauthorized();
            }

            var userId = userInfo.UserId ?? 0;

            if (userInfo.IsAdmin)
            {
                var results = service.GetEnrollmentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);
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

                var results = service.GetEnrollmentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = service.GetEnrollmentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = service.GetEnrollmentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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
    }
}
