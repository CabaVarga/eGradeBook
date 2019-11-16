using eGradeBook.Models.Dtos.StudentParents;
using eGradeBook.Services;
using eGradeBook.Utilities.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/studentparents")]
    [Authorize]
    public class StudentParentsController : ApiController
    {
        private IStudentParentsService studentParentsService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public StudentParentsController(IStudentParentsService studentParentsService)
        {
            this.studentParentsService = studentParentsService;
        }

        [Route("{studentParentId}", Name = "GetStudentParent")]
        [HttpGet]
        public IHttpActionResult GetStudentParentById(int studentParentId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get StudentParent {@studentParentId} by {@userData}", studentParentId, userData);

            return Ok(studentParentsService.GetStudentParentById(studentParentId));
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllStudentParents()
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get all StudentParents by {@userData}", userData);

            return Ok(studentParentsService.GetAllStudentParents());
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult PostCreateStudentParent(StudentParentDto studentParentDto)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create StudentParent {@studentParentDto} by {@userData}", studentParentDto, userData);

            var result = studentParentsService.CreateStudentParent(studentParentDto);

            logger.Info("Created StudentParent {@StudentParentId}", result.StudentParentId);

            return CreatedAtRoute("GetStudentParent", new { studentParentId = result.StudentParentId }, result);
        }

        #region QUERY
        /// <summary>
        /// Get studentparents based on query
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
        public IHttpActionResult GetStudentParentsByQuery(
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
                var results = studentParentsService.GetStudentParentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = studentParentsService.GetStudentParentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = studentParentsService.GetStudentParentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = studentParentsService.GetStudentParentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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


        [Route("{studentParentId}")]
        [HttpDelete]
        public IHttpActionResult DeleteStudentParent(int studentParentId)
        {
            var deleted = studentParentsService.DeleteStudentParentForReal(studentParentId);

            if (deleted == null)
            {
                return null;
            }

            return Ok(deleted);
        }
        #endregion
    }
}
