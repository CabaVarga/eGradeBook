using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Models.Dtos.Teachings;
using eGradeBook.Services;
using eGradeBook.SwaggerHelpers.Examples;
using NLog;
using Swashbuckle.Examples;
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
    /// Web api for teaching assignments
    /// NOTE: superfluous (obsolete). Functionality will be in api/teachers, api/courses
    /// NOTE: maybe not
    /// </summary>
    [RoutePrefix("api/teachings")]
    [Authorize(Roles = "admins")]
    public class TeachingsController : ApiController
    {
        private ITeachingsService teachings;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="teachings"></param>
        public TeachingsController(ITeachingsService teachings)
        {
            this.teachings = teachings;
        }

        /// <summary>
        /// Create teaching assignment
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        [Route("")]
        [SwaggerRequestExample(typeof(TeachingDto), typeof(CreateTeachingExample))]
        [HttpPost]
        public IHttpActionResult CreateTeachingAssignment(TeachingDto teachingDto)
        {
            logger.Info("Create teaching assignment {@teachingData}", teachingDto);

            var teaching = teachings.CreateTeachingDto(teachingDto);

            return CreatedAtRoute("GetTeaching", new { teachingId = teaching.TeachingId }, teaching);
        }

        /// <summary>
        /// Get all teachings
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [ResponseType(typeof(IEnumerable<TeachingDto>))]
        public IHttpActionResult GetTeachings()
        {
            logger.Info("Get all teaching");
            return Ok(teachings.GetAllTeachingsDtos());
        }

        /// <summary>
        /// Get teaching by Id --- throws if not found
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        [Route("{teachingId}", Name = "GetTeaching")]
        [HttpGet]
        [ResponseType(typeof(TeachingDto))]
        public IHttpActionResult GetTeachingById(int teachingId)
        {
            logger.Info("Get teaching by Id {@teachingId}", teachingId);

            return Ok(teachings.GetTeachingDtoById(teachingId));
        }

        /// <summary>
        /// Get all teaching assignments of a teacher
        /// </summary>
        /// <returns></returns>
        [Route("by-teachers")]
        public IHttpActionResult GetTeachingAssignmentsByTeachers()
        {
            return Ok(teachings.GetAllTeachingAssignmentsByTeachers());
        }

        /// <summary>
        /// Get all teaching assignments of a course
        /// </summary>
        /// <returns></returns>
        [Route("by-courses")]
        public IHttpActionResult GetTeachingAssignmentsByCourses()
        {
            return Ok(teachings.GetAllTeachingAssignmentsByCourses());
        }
        /// <summary>
        /// Remove teaching assignment
        /// </summary>
        /// <param name="assignment"></param>
        /// <returns></returns>
        [Route("remove-assignment")]
        [HttpPut]
        public IHttpActionResult RemoveTeachingAssignment(TeachingAssignmentDto assignment)
        {
            teachings.RemoveTeacherFromCourse(assignment.SubjectId, assignment.TeacherId);

            return Ok();
        }
    }
}
