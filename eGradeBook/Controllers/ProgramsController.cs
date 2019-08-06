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
    /// Temporary Api for programs
    /// NOTE this will not remain in the final api!
    /// NOTE final grades will also only be an aspect of either takings or grades... but i'm not completely convinced yet of that
    /// </summary>
    [RoutePrefix("api/programs")]
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
        /// Get programs grouped by courses
        /// </summary>
        /// <returns></returns>
        [Route("by-courses")]
        public IHttpActionResult GetProgramsByCourses()
        {
            return Ok(programs.GetAllProgramsGroupedByCourses());
        }

        /// <summary>
        /// Get programs grouped by classrooms
        /// </summary>
        /// <returns></returns>
        [Route("by-classes")]
        public IHttpActionResult GetProgramsBySchoolClasses()
        {
            return Ok(programs.GetAllProgramsGroupedBySchoolClasses());
        }

    }
}
