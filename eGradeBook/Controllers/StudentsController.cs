using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        private IStudentsService service;

        public StudentsController(IStudentsService service)
        {
            this.service = service;
        }

        public IHttpActionResult GetAllStudents()
        {
            return Ok(service.GetAllStudentsDto());
        }

        [Route("{studentId}", Name = "GetStudentById")]
        public IHttpActionResult GetStudentById(int studentId)
        {
            return Ok(service.GetStudentByIdDto(studentId));
        }
    }
}
