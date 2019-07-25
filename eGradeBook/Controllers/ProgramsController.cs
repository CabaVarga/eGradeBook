using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/programs")]
    public class ProgramsController : ApiController
    {
        private IProgramsService programs;

        public ProgramsController(IProgramsService programs)
        {
            this.programs = programs;
        }

        [Route("by-courses")]
        public IHttpActionResult GetProgramsByCourses()
        {
            return Ok(programs.GetAllProgramsGroupedByCourses());
        }

        [Route("by-classes")]
        public IHttpActionResult GetProgramsBySchoolClasses()
        {
            return Ok(programs.GetAllProgramsGroupedBySchoolClasses());
        }

    }
}
