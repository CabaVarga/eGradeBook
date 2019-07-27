using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/finalgrades")]
    public class FinalGradesController : ApiController
    {
        private IFinalGradesService service;

        public FinalGradesController(IFinalGradesService service)
        {
            this.service = service;
        }

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
