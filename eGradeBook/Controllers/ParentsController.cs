using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/parents")]
    public class ParentsController : ApiController
    {
        private IParentsService service;

        public ParentsController(IParentsService service)
        {
            this.service = service;
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllParentsWithChildren()
        {
            return Ok(service.GetAllParentsWithTheirChildrent());
        }

        [Route("{parentId}/children")]
        [HttpGet]
        public IHttpActionResult GetChildrenOfParent(int parentId)
        {
            return Ok(service.GetAllChildren(parentId));
        }

        [Route("{parentId}/children")]
        [HttpPost]
        public IHttpActionResult AddStudentAsChild(int parentId, [FromUri]int studentId)
        {
            return Ok(service.AddChild(parentId, studentId));
        } 

        [Route("parents-for-students")]
        [HttpGet]
        public IHttpActionResult GetParentsForStudents()
        {
            return Ok(service.GetParentsForStudents());
        }
    }
}
