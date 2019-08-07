using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Courses;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Teachings;
using eGradeBook.Services;
using eGradeBook.Utilities.WebApi;
using NLog;
using System;
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
        private ICoursesService service;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public CoursesController(ICoursesService service)
        {
            this.service = service;
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

            var courses = service.GetAllCoursesDto();

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

            var course = service.GetCourseDtoById(courseId);

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
            var createdCourse = service.CreateCourse(course);

            var link = Url.Route("DefaultApi", new { controller = "Courses", courseId = createdCourse.Id });

            logger.Info("Course {@courseId} created at route {@route}", createdCourse.Id, link);

            // TODO ERROR HANDLING
            return CreatedAtRoute("DefaultApi", new { controller = "Courses", courseId = createdCourse.Id }, createdCourse);
        }

        #region Teachings
        [Route("{courseId}/teachers")]
        [HttpGet]
        public IHttpActionResult GetTeachingAssignments(int courseId)
        {
            return Ok(service.GetAllTeachings(courseId));
        }

        [Route("{courseId}/teachers/{teacherId}")]
        [HttpGet]
        public IHttpActionResult GetTeachingAssignments(int courseId, int teacherId)
        {
            return Ok(service.GetTeaching(courseId, teacherId));
        }

        [Route("{courseId}/teachers")]
        [HttpPost]
        public IHttpActionResult CreateTeachingAssignment(int courseId, TeachingDto teaching)
        {
            return Ok(service.CreateTeachingAssignment(teaching));
        }
        #endregion

        #region Programs
        [Route("{courseId}/teachers/{teacherId}/classrooms")]
        [HttpGet]
        public IHttpActionResult GetAllClassRoomsPrograms(int courseId, int teacherId)
        {
            logger.Info("Get all programs for course {@courseId} and teacher {@teacherId}", courseId, teacherId);

            return Ok(service.GetAllPrograms(courseId, teacherId));
        }

        [Route("{courseId}/teachers/{teacherId}/classrooms/{classRoomId}")]
        [HttpGet]
        public IHttpActionResult GetClassRoomProgram(int courseId, int teacherId, int classRoomId)
        {
            logger.Info("Get program for course {@courseId} teacher {@teacherId} and classRoom {@classRoomId}", courseId, teacherId, classRoomId);

            return Ok(service.GetProgram(courseId, teacherId, classRoomId));
        }

        [Route("{courseId}/teachers/{teacherId}/classrooms")]
        [HttpPost]
        public IHttpActionResult CreateClassRoomProgram(ProgramDto dto)
        {
            logger.Info("Create Program {@programData}", dto);

            ProgramDto createdProgram = service.CreateProgram(dto);

            var link = Url.Route("DefaultApi", new
            {
                controller = "GetClassRoomProgram",
                courseId = createdProgram.CourseId,
                teacherId = createdProgram.TeacherId,
                classRoomId = createdProgram.ClassRoomId
            });

            Uri.TryCreate(link, UriKind.Absolute, out Uri uri);

            logger.Info("Program {@programId} created at route {@route}", createdProgram.ClassRoomId, link);

            return Created(uri, createdProgram); 
        }
        #endregion

        #region Takings

        #endregion
    }
}
