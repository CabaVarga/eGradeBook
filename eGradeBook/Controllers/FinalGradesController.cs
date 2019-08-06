using eGradeBook.Services;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Web api controller for working with final grades
    /// </summary>
    [RoutePrefix("api/finalgrades")]
    public class FinalGradesController : ApiController
    {
        /// <summary>
        /// The service for business logic
        /// </summary>
        private IFinalGradesService service;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The controller constructor
        /// </summary>
        /// <param name="service"></param>
        public FinalGradesController(IFinalGradesService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Retrieve final grades for a given course.
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [Route("by-course")]
        public IHttpActionResult GetFinalGradesByCourse(int courseId)
        {
            try
            {
                return Ok(service.GetAllFinalGradesForCourse(courseId));
            }

            catch 
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Retrieve final grades for a given student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [Route("by-student/{studentId}")]
        public IHttpActionResult GetFinalGradesByStudent(int studentId)
        {
            try
            {
                return Ok(service.GetAllFinalGradesForStudent(studentId));
            }

            catch
            {
                return NotFound();
            }
        }
    }
}
