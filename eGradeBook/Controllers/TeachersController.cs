using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        /// <summary>
        /// Return all teachers in the system.
        /// </summary>
        /// <returns>A Json array of Teacher Dtos</returns>
        [Route("")]
        [ResponseType(typeof(TeacherDto))]
        [HttpGet]
        public IHttpActionResult GetAllTeachers()
        {
            return Ok(teachersService.GetAllTeachersDtos());
        }

        /// <summary>
        /// Retrieve a teacher by its Id.
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns>A Json object of the Teacher Dto</returns>
        [Route("{teacherId:int}")]
        [ResponseType(typeof(TeacherDto))]
        [HttpGet]
        public IHttpActionResult GetTeacherById(int teacherId)
        {
            return Ok(teachersService.GetTeacherByIdDto(teacherId));
        }

        /// <summary>
        /// Assign a Course to the given teacher.
        /// </summary>
        /// <param name="teacherId">The Id of the teacher</param>
        /// <param name="assignment">A Teaching Assignment Dto Json object</param>
        /// <returns>Status code of 200 in case of success</returns>
        [Route("{teacherId:int}/courses")]
        [ResponseType(typeof(void))]
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

        // TODO Add the rest of CRUD methods. We have create (in account), read (all and one), we need update and delete.
        // Watch what will happen in UserRole if I delete a subject... Can I even delete without clearing that first?
        // Should I even bother with changes here or every change should be done through Identity / UserService?

        // Let's add them here. In case of the other approach i will refactor.

        /// <summary>
        /// Update personal data for given teacher.
        /// </summary>
        /// <param name="teacherId">Id of teacher</param>
        /// <param name="teacher">Json object of type Teacher Dto</param>
        /// <returns>A Json object of the Teacher Dto with the updated personal details</returns>
        [HttpPut]
        [Route("{teacherId}")]
        [ResponseType(typeof(TeacherDto))]
        public IHttpActionResult PutUpdateTeacher(int teacherId, TeacherDto teacher)
        {
            // check model

            // check id

            // call service

            // return updated teacher
            return Ok(teachersService.UpdateTeacher(teacherId, teacher));
            // catch exception: here or at some central place?
        }

        /// <summary>
        /// Delete a teacher with a given Id
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns>A Json object of type Teacher Dto for the deleted teacher</returns>
        [HttpDelete]
        [Route("{teacherId}")]
        [ResponseType(typeof(TeacherDto))]
        public IHttpActionResult DeleteTeacher(int teacherId)
        {
            // check model

            // check id

            // call service

            // return deleted teacher
            return Ok(teachersService.DeleteTeacher(teacherId));
            // catch exception: here or at some central place?
        }

        // TODO Crud for admin, student and parent
        // TODO Prepare methods in services
    }
}