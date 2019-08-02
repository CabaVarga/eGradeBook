using eGradeBook.Models.Dtos.Students;
using eGradeBook.Services;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public StudentsController(IStudentsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllStudents()
        {
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
            return Ok(service.GetStudentsByNameStartingWith(firstName));
        }

    }
}
