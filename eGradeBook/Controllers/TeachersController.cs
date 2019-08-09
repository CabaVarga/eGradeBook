using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Services;
using eGradeBook.Utilities.WebApi;
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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TeachersController(ITeachersService service)
        {
            this.teachersService = service;
        }

        /// <summary>
        /// Return all teachers in the system.
        /// </summary>
        /// <returns>A Json array of Teacher Dtos</returns>
        [Route("")]
        [ResponseType(typeof(IEnumerable<TeacherDto>))]
        [HttpGet]
        public IHttpActionResult GetAllTeachers()
        {
            var user = IdentityHelper.GetLoggedInUser(RequestContext);
            logger.Info("User {@userData} is requesting a list of all teacher", user);

            var teachers = teachersService.GetAllTeachersDtos();

            return Ok(teachers);
        }

        /// <summary>
        /// Retrieve a teacher by its Id.
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns>A Json object of the Teacher Dto</returns>
        [Route("{teacherId:int}", Name = "GetTeacherById")]
        [ResponseType(typeof(TeacherDto))]
        [HttpGet]
        public IHttpActionResult GetTeacherById(int teacherId)
        {
            var user = IdentityHelper.GetLoggedInUser(RequestContext);
            logger.Info("User {@userData} is requesting a teacher by Id {teacherId}", user, teacherId);

            var teacher = teachersService.GetTeacherByIdDto(teacherId);

            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
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
            var user = IdentityHelper.GetLoggedInUser(RequestContext);
            logger.Info("User {@userData} is requesting a teaching assignment creation {assignmentData}", user, assignment);

            if (assignment.TeacherId != teacherId)
            {
                logger.Error("Provided Teacher identity does not match the assignment values.");
                return BadRequest("Teacher identities do not match.");
            }

            // I'm not sure how to handle these cases
            // Maybe Mladen's solution can help? Probably not...
            // Read up again about the subject...
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
        /// NOTE this will go to account!
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
        /// NOTE this will go to account!
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

        /// <summary>
        /// Return a list of courses with the classrooms and a list of classrooms with the courses
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        [Route("{teacherId}/extended")]
        [HttpGet]
        [ResponseType(typeof(TeacherExtendedDto))]
        public IHttpActionResult GetTeacherExtendedData(int teacherId)
        {
            return Ok(teachersService.GetExtendedDataForTeacher(teacherId));
        }

        // TODO Crud for admin, student and parent
        // TODO Prepare methods in services


        [Route("{teacherId}/courses")]
        [HttpGet]
        public IHttpActionResult GetCoursesForTeacher(int teacherId)
        {
            return Ok(teachersService.GetCoursesForTeacher(teacherId));
        }

        [Route("{teacherId}/courses-and-classrooms")]
        [HttpGet]
        public IHttpActionResult GetCoursesClassroomsForTeacher(int teacherId)
        {
            return Ok(teachersService.GetCoursesClassRoomsForTeacher(teacherId));
        }

        [Route("{teacherId}/classrooms")]
        [HttpGet]
        public IHttpActionResult GetClassroomsForTeacher(int teacherId)
        {
            return Ok(teachersService.GetClassRoomsForTeacher(teacherId));
        }

        [Route("{teacherId}/classrooms-and-courses")]
        [HttpGet]
        public IHttpActionResult GetClassroomsCoursesForTeacher(int teacherId)
        {
            return Ok(teachersService.GetClassRoomsCoursesForTeacher(teacherId));
        }

        [Route("{teacherId}/report")]
        [HttpGet]
        public IHttpActionResult GetTeacherReport(int teacherId)
        {
            // TODO authr

            return Ok(teachersService.GetTeacherReport(teacherId));
        }
    }
}