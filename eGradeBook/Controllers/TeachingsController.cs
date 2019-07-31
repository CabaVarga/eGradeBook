using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Web api for teaching assignments
    /// NOTE: superfluous (obsolete). Functionality will be in api/teachers, api/courses
    /// </summary>
    [RoutePrefix("api/teachings")]
    public class TeachingsController : ApiController
    {
        private ITeachingsService teachings;

        public TeachingsController(ITeachingsService teachings)
        {
            this.teachings = teachings;
        }

        [Route("by-teachers")]
        public IHttpActionResult GetTeachingAssignmentsByTeachers()
        {
            return Ok(teachings.GetAllTeachingAssignmentsByTeachers());
        }

        [Route("by-courses")]
        public IHttpActionResult GetTeachingAssignmentsByCourses()
        {
            return Ok(teachings.GetAllTeachingAssignmentsByCourses());
        }
    }
}
