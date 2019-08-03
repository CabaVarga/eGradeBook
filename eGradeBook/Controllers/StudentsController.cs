using eGradeBook.Models.Dtos.Students;
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
    /// <summary>
    /// Web api controller for student users
    /// </summary>
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        private IStudentsService service;
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public StudentsController(IStudentsService service, ILogger logger)
        {
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllStudents()
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);

            logger.Info("User {@user} requested all students",
                IdentityHelper.GetLoggedInUser(userData));

            return Ok(service.GetAllStudentsDto());
        }

        /// <summary>
        /// Get a student by Id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet]
        [ResponseType(typeof(StudentDto))]
        [Route("{studentId}", Name = "GetStudentById")]
        public IHttpActionResult GetStudentById(int studentId)
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);

            logger.Info("User {@user} requested student with Id {studentId}",
                IdentityHelper.GetLoggedInUser(userData), studentId);
            return Ok(service.GetStudentById(studentId));
        }

        /// <summary>
        /// Get students by their firstname
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        [Route("by-firstname/{firstName}")]
        [HttpGet]
        public IHttpActionResult GetStudentsByFirstName(string firstName)
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);

            logger.Info("User {@user} requested students with first name {firstName}",
                IdentityHelper.GetLoggedInUser(userData), firstName);

            return Ok(service.GetStudentsByFirstNameStartingWith(firstName));
        }

        /// <summary>
        /// Get students by their lastname
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [Route("by-lastname/{lastName}")]
        [ResponseType(typeof(IEnumerable<StudentDto>))]
        [HttpGet]
        public IHttpActionResult GetStudentsByLastName(string lastName)
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);
            logger.Info("User {@user} requested students with last name {lastName}",
                IdentityHelper.GetLoggedInUser(userData), lastName);

            return Ok(service.GetStudentsByLastNameStartingWith(lastName));
        }


        /// <summary>
        /// Get students with their parents
        /// </summary>
        /// <returns></returns>
        [Route("with-parents")]
        [ResponseType(typeof(IEnumerable<StudentWithParentsDto>))]
        [HttpGet]
        public IHttpActionResult GetStudentsWithParents()
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);

            logger.Info("User {@user} requested retrieval of all students with their parents",
                IdentityHelper.GetLoggedInUser(userData));

            return Ok(service.GetAllStudentsWithParents());
        }

        [Route("{studentId}/courses")]
        [HttpPost]
        public IHttpActionResult PostAssignCourseToStudent(int studentId, StudentCourseDto course)
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);
            logger.Info("User {@user} requested assignment of course {@assignment} to student {studentId}",
                IdentityHelper.GetLoggedInUser(userData), course, studentId);

            service.AssignCourseToStudent(course);

            return Ok();
        }
    }
}
