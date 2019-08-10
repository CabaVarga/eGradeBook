using eGradeBook.Models.Dtos.Programs;
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
    /// Temporary Api for programs
    /// NOTE this will not remain in the final api!
    /// NOTE final grades will also only be an aspect of either takings or grades... but i'm not completely convinced yet of that
    /// </summary>
    [RoutePrefix("api/programs")]
    [Authorize(Roles = "admins")]
    public class ProgramsController : ApiController
    {
        private IProgramsService programs;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="programs"></param>
        public ProgramsController(IProgramsService programs)
        {
            this.programs = programs;
        }

        /// <summary>
        /// Create new program
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [SwaggerRequestExample(typeof(ProgramDto), typeof(CreateProgramExample))]
        public IHttpActionResult PostCreateProgram(ProgramDto programDto)
        {
            logger.Info("Create program {@programData}", programDto);

            var createdProgram = programs.CreateProgramDto(programDto);

            return CreatedAtRoute("GetProgram", new { programId = createdProgram.ProgramId }, createdProgram);
        }

        /// <summary>
        /// Get program by Id
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [Route("{programId}", Name = "GetProgram")]
        [HttpGet]
        [ResponseType(typeof(ProgramDto))]
        public IHttpActionResult GetProgram(int programId)
        {
            logger.Info("Get program {@programId}", programId);

            return Ok(programs.GetProgramDto(programId));
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllProgramsDtos()
        {
            logger.Info("Get all programs");

            return Ok(programs.GetAllProgramsDtos());
        }

        [Route("{programId}")]
        [HttpPut]
        public IHttpActionResult UpdateProgram(int programId, ProgramDto programDto)
        {
            logger.Info("Update program {@programId} {@programData}", programId, programDto);

            if (programId != programDto.ProgramId)
            {
                return BadRequest("Id mismatch");
            }

            ProgramDto updatedProgram = programs.UpdateProgramDto(programDto);

            return Ok(updatedProgram);
        }

        /// <summary>
        /// Get programs grouped by courses
        /// </summary>
        /// <returns></returns>
        [Route("by-courses")]
        public IHttpActionResult GetProgramsByCourses()
        {
            logger.Info("Get programs grouped by courses");
            return Ok(programs.GetAllProgramsGroupedByCourses());
        }

        /// <summary>
        /// Get programs grouped by classrooms
        /// </summary>
        /// <returns></returns>
        [Route("by-classes")]
        public IHttpActionResult GetProgramsBySchoolClasses()
        {
            logger.Info("Get programs grouped by classrooms");
            return Ok(programs.GetAllProgramsGroupedBySchoolClasses());
        }

    }
}
