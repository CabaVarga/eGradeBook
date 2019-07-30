using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/teachers")]
    public class TeachersController : ApiController
    {
        private ITeachersService teachersService;
        private ILogger logger;

        public TeachersController(ITeachersService service, ILogger logger)
        {
            this.teachersService = service;
            this.logger = logger;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllTeachers()
        {
            return Ok(teachersService.GetAllTeachersDtos());
        }


        [Route("{teacherId:int}")]
        [ResponseType(typeof(TeacherDto))]
        [HttpGet]
        public IHttpActionResult GetTeacherById(int teacherId)
        {
            return Ok(teachersService.GetTeacherByIdDto(teacherId));
        }

        [Route("{teacherId:int}/courses")]
        [HttpPost]
        public IHttpActionResult PostAssignCourseToTeacher(int teacherId, TeachingAssignmentDto assignment)
        {
            if (assignment.TeacherId != teacherId)
            {
                logger.Error("Teacher identities do not match at teaching assignment creation command.");
                return BadRequest("Teacher identities do not match.");
            }

            teachersService.AssignCourseToTeacher(assignment);
            logger.Info("Teaching assignment successfully created.");
            return Ok();
        }
    }
}