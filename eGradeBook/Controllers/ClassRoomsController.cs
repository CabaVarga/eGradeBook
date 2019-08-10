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
        public IHttpActionResult PostCreateClassRoom(ClassRoomRegistrationDto classRoom)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create ClassRoom {@classRoomData} by {@userData}", classRoom, userData);

            return Ok(service.CreateClassRoom(classRoom));
        }

        /// <summary>
        /// Create a new program entry associated with the given classroom
        /// </summary>
        /// <param name="classRoomId">Class room Id</param>
        /// <param name="program">Class room program dto</param>
        /// <returns></returns>
        [Route("{classRoomId:int}/programs")]
        [HttpPost]
        [SwaggerRequestExample(typeof(ClassRoomProgramDto), typeof(CreateClassRoomProgramExample))]
        public IHttpActionResult PostCreateProgramOfClassRoom(int classRoomId, ClassRoomProgramDto program)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create Program {@programData} for ClassRoom {@classRoomId} by {@userData}", program, classRoomId, userData);

            service.CreateClassRoomProgram(program);

            return Ok();
        }
    }
}
