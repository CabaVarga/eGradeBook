using eGradeBook.Models.Dtos.Takings;
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

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Takings controller
    /// </summary>
    [RoutePrefix("api/takings")]
    [Authorize(Roles = "admins")]
    public class TakingsController : ApiController
    {
        private ITakingsService takingsService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="takingsService"></param>
        public TakingsController(ITakingsService takingsService)
        {
            this.takingsService = takingsService;
        }

        /// <summary>
        /// Get taking by Id
        /// </summary>
        /// <param name="takingId"></param>
        /// <returns></returns>
        [Route("{takingId}", Name = "GetTaking")]
        [HttpGet]
        public IHttpActionResult GetTaking(int takingId)
        {
            logger.Info("Get taking by Id {@takingId}", takingId);

            return Ok(takingsService.GetTakingDtoById(takingId));
        }

        /// <summary>
        /// Get all takings
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllTakingsDtos()
        {
            logger.Info("Get all takings");

            return Ok(takingsService.GetAllTakingsDtos());
        }

        // NOTE no update (at the moment. If a taking has a startDate and endDate, then yes.

        /// <summary>
        /// Create new taking
        /// </summary>
        /// <param name="takingDto"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [SwaggerRequestExample(typeof(TakingDto), typeof(CreateTakingForStudentExample))]
        public IHttpActionResult PostCreateTaking(TakingDto takingDto)
        {
            logger.Info("Create taking {@takingData}", takingDto);

            TakingDto taking = takingsService.CreateTakingDto(takingDto);

            return CreatedAtRoute("GetTaking", new { takingId = taking.TakingId }, taking);
        }
    }
}
