using eGradeBook.Models.Dtos.ClassRooms;
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
    /// Web api controller for working with classrooms related data and relations
    /// </summary>
    [RoutePrefix("api/classrooms")]
    [Authorize]
    public class ClassRoomsController : ApiController
    {
        /// <summary>
        /// Business logic
        /// </summary>
        private IClassRoomsService service;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public ClassRoomsController(IClassRoomsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// The R in CRUD, retrieve all classrooms
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetAllClassRooms()
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get all ClassRooms by {@userData}", userData);

            return Ok(service.GetAllClassRooms());
        }

        /// <summary>
        /// Retrieve a classroom by the given Id
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        [Route("{classRoomId:int}")]
        [Authorize(Roles = "admins")]
        [HttpGet]
        public IHttpActionResult GetClassRoomById(int classRoomId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get ClassRoom {@classRoomId} by {@userData}", classRoomId, userData);

            return Ok(service.GetClassRoomById(classRoomId));
        }

        /// <summary>
        /// Used to enroll a student in a given classroom
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <param name="enroll"></param>
        /// <returns></returns>
        [Route("{classRoomId}/enrollments")]
        [Authorize(Roles = "admins")]
        [SwaggerRequestExample(typeof(ClassRoomEnrollStudentDto), typeof(EnrollStudentInClassRoomExample))]
        [HttpPost]
        public IHttpActionResult PostEnrollStudent(int classRoomId, ClassRoomEnrollStudentDto enroll)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Enroll Student {@studentId} in ClassRoom {@classRoomId} {@enrollData} by {@userData}", enroll.StudentId, enroll.ClassRoomId, enroll, userData);

            if (classRoomId != enroll.ClassRoomId)
            {
                return BadRequest("Class identities do not match.");
            }

            return Ok(service.EnrollStudent(enroll));
        }

        /// <summary>
        /// Create a new classroom
        /// </summary>
        /// <param name="classRoom"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [Authorize(Roles = "admins")]
        public IHttpActionResult PostCreateClassRoom(ClassRoomRegistrationDto classRoom)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create ClassRoom {@classRoomData} by {@userData}", classRoom, userData);

            return Ok(service.CreateClassRoom(classRoom));
        }

        /// <summary>
        /// Delete classroom
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        [Route("{classRoomId}")]
        [HttpDelete]
        public ClassRoomDto DeleteClassRoom(int classRoomId)
        {
            var deletedClassroom = service.DeleteClassRoom(classRoomId);

            if (deletedClassroom == null)
            {
                return null;
            }

            return deletedClassroom;
        }

        [Route("{classRoomId}")]
        [HttpPut]
        public IHttpActionResult UpdateClassRoom(int classRoomId, ClassRoomDto classRoomDto)
        {
            logger.Info("Update classroom {@programId} {@programData}", classRoomId, classRoomDto);

            if (classRoomId != classRoomDto.ClassRoomId)
            {
                return BadRequest("Id mismatch");
            }

            var updatedClassRoom = service.UpdateClassRoom(classRoomId, classRoomDto);

            return Ok(updatedClassRoom);
        }

        /// <summary>
        /// Create a new program entry associated with the given classroom
        /// </summary>
        /// <param name="classRoomId">Class room Id</param>
        /// <param name="program">Class room program dto</param>
        /// <returns></returns>
        [Route("{classRoomId:int}/programs")]
        [HttpPost]
        [Authorize(Roles = "admins")]
        [SwaggerRequestExample(typeof(ClassRoomProgramDto), typeof(CreateClassRoomProgramExample))]
        public IHttpActionResult PostCreateProgramOfClassRoom(int classRoomId, ClassRoomProgramDto program)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create Program {@programData} for ClassRoom {@classRoomId} by {@userData}", program, classRoomId, userData);

            service.CreateClassRoomProgram(program);

            return Ok();
        }


        #region Reports
        [Route("{classRoomId}/basic-report")]
        [HttpGet]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetClassRoomBasicReport(int classRoomId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Basic Report for ClassRoom {@classRoomId} by {@userData}", classRoomId, userData);

            var report = service.GetBasicReport(classRoomId);

            return Ok(report);
        }

        [Route("{classRoomId}/full-report")]
        [HttpGet]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetClassRoomFullReport(int classRoomId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Basic Report for ClassRoom {@classRoomId} by {@userData}", classRoomId, userData);

            var report = service.GetFullReport(classRoomId);

            return Ok(report);
        }
        #endregion

        #region QUERY
        /// <summary>
        /// Get classrooms based on query
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
        public IHttpActionResult GetClassRoomsByQuery(
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
                var results = service.GetClassRoomsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = service.GetClassRoomsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = service.GetClassRoomsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = service.GetClassRoomsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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
