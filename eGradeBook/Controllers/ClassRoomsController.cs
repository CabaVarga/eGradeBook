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
    [RoutePrefix("api/classrooms")]
    public class ClassRoomsController : ApiController
    {
        private IClassRoomsService service;

        public ClassRoomsController(IClassRoomsService service)
        {
            this.service = service;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllClassRooms()
        {
            return Ok(service.GetAllClassRooms());
        }

        [Route("{classRoomId:int}")]
        [HttpGet]
        public IHttpActionResult GetClassRoomById(int classRoomId)
        {
            return Ok(service.GetClassRoomById(classRoomId));
        }

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

        [Route("")]
        [HttpPost]
        public IHttpActionResult PostCreateClassRoom(ClassRoomRegistrationDto classRoom)
        {
            return Ok(service.CreateClassRoom(classRoom));
        }
    }
}
