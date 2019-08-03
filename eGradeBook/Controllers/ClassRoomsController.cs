using eGradeBook.Models.Dtos.ClassRooms;
using eGradeBook.Services;
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
            return Ok(service.GetClassRoomById(classRoomId));
        }

        /// <summary>
        /// Used to enroll a student in a given classroom
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <param name="enroll"></param>
        /// <returns></returns>
        [Route("{classRoomId}/enrollments")]
        [HttpPost]
        public IHttpActionResult PostEnrollStudent(int classRoomId, ClassRoomEnrollStudentDto enroll)
        {
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
        public IHttpActionResult PostCreateProgramOfClassRoom(int classRoomId, ClassRoomProgramDto program)
        {
            service.CreateClassRoomProgram(program);

            return Ok();
        }
    }
}
