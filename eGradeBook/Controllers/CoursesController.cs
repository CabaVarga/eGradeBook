using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Courses;
using eGradeBook.Services;
using eGradeBook.Utilities.WebApi;
using NLog;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Web api for working with Courses
    /// </summary>
    [RoutePrefix("api/courses")]
    public class CoursesController : ApiController
    {
        private ICoursesService coursesService;
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public CoursesController(ICoursesService service, ILogger logger)
        {
            this.coursesService = service;
            this.logger = logger;
        }

        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<CourseDto>))]
        public IHttpActionResult GetAllCourses()
        {
            var user = IdentityHelper.GetLoggedInUser(RequestContext);
            logger.Info("User {@userData} is requesting a list of all courses", user);

            var courses = coursesService.GetAllCoursesDto();

            // TODO this is not right
            if (courses == null)
            {
                return NotFound();
            }

            return Ok(courses);
        }

        /// <summary>
        /// Get a course by Id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [Route("{courseId:int}")]
        [HttpGet]
        [ResponseType(typeof(CourseDto))]
        public IHttpActionResult GetCourseById(int courseId)
        {
            var user = IdentityHelper.GetLoggedInUser(RequestContext);
            logger.Info("User {@userData} is requesting a course by Id {courseId}", user, courseId);

            var course = coursesService.GetCourseDtoById(courseId);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        /// <summary>
        /// Register a new course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        [Route("")]
        [HttpPost]
        public IHttpActionResult RegisterCourse(CourseDto course)
        {
            var user = IdentityHelper.GetLoggedInUser(RequestContext);
            logger.Info("User {@userData} is initiating a Course registration {@courseData}", user, course);

            // TODO logging
            var createdCourse = coursesService.CreateCourse(course);

            var link = Url.Route("DefaultApi", new { controller = "Courses", courseId = createdCourse.Id });

            logger.Info("Course {@courseId} created at route {@route}", createdCourse.Id, link);

            // TODO ERROR HANDLING
            return CreatedAtRoute("DefaultApi", new { controller = "Courses", courseId = createdCourse.Id }, createdCourse);
        }
    }
}
