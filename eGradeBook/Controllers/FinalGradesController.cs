using eGradeBook.Models.Dtos.FinalGrades;
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
    /// Web api controller for working with final grades
    /// </summary>
    [RoutePrefix("api/finalgrades")]
    [Authorize(Roles = "admins")]
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
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Final Grades for Course {@courseId} by {@userData}", courseId, userData);

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
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Final Grades for Student {@studentId} by {@userData}", studentId, userData);

            try
            {
                return Ok(service.GetAllFinalGradesForStudent(studentId));
            }

            catch
            {
                return NotFound();
            }
        }

        #region CRUD
        [HttpPost]
        [ResponseType(typeof(FinalGradeDto))]
        [Route("")]
        public IHttpActionResult CreateFinalGrade(FinalGradeDto finalGradeDto)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create Final Grade {@finalGradeDto} by {@userData}", finalGradeDto, userData);

            var result = service.CreateFinalGrade(finalGradeDto);

            if (result == null)
            {
                return NotFound();
            }

            logger.Info("Created Final Grade {@finalGradeId}", result.FinalGradeId);

            return Ok(result);
        }


        [HttpGet]
        [ResponseType(typeof(IEnumerable<FinalGradeDto>))]
        [Route("")]
        public IHttpActionResult GetFinalGrades()
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Requestad all Final Grades by {@userData}", userData);

            var result = service.GetAllFinalGradesDto();

            if (result == null)
            {
                return NotFound();
            }

            logger.Info("Received all Final Grades ");

            return Ok(result);
        }


        [HttpDelete]
        [ResponseType(typeof(FinalGradeDto))]
        [Route("{finalGradeId}")]
        public IHttpActionResult DeleteFinalGrade(int finalGradeId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Delete Final Grade {@finalGradeId} by {@userData}", finalGradeId, userData);

            var result = service.DeleteFinalGrade(finalGradeId);

            if (result == null)
            {
                return NotFound();
            }

            logger.Info("Deleted Final Grade {@finalGradeId}", result.FinalGradeId);

            return Ok(result);
        }



        #endregion
    }
}
