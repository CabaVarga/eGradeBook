using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

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

        [Route("{parentId:int}", Name = "GetParentById")]
        [HttpGet]
        public IHttpActionResult GetParentById(int parentId)
        {
            return Ok(service.GetParentById(parentId));
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

        #region Update & Delete

        [HttpPut]
        [Route("{parentId}")]
        [ResponseType(typeof(ParentDto))]
        public IHttpActionResult PutUpdateParent(int parentId, ParentDto parent)
        {
            return Ok(service.UpdateParent(parentId, parent));
        }

        [HttpDelete]
        [Route("{parentId}")]
        [ResponseType(typeof(ParentDto))]
        public IHttpActionResult DeleteParent(int parentId)
        {
            return Ok(service.DeleteParent(parentId));
        }

        #endregion
    }
}
