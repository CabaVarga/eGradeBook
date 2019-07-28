using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Courses;
using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/courses")]
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

        [Authorize(Roles = "admins")]
        [Route("")]
        [HttpPost]
        public IHttpActionResult RegisterCourse(CourseDto course)
        {
            var createdCourse = coursesService.CreateCourse(course);

            var link = Url.Route("DefaultApi", new { controller = "Courses", courseId = createdCourse.Id });

            return CreatedAtRoute("DefaultApi", new { controller = "Courses", courseId = createdCourse.Id }, createdCourse);
        }
    }
}
