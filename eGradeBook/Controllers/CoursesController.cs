using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    public class CoursesController : ApiController
    {
        private ICoursesService coursesService;

        public CoursesController(ICoursesService service)
        {
            this.coursesService = service;
        }

        public IHttpActionResult GetAllCourses()
        {
            return Ok(coursesService.GetAllCourses());
        }
    }
}
