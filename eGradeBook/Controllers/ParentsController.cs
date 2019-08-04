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
    /// <summary>
    /// Web api controller for parent users and related tasks
    /// </summary>
    [RoutePrefix("api/parents")]
    public class ParentsController : ApiController
    {
        private IParentsService service;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public ParentsController(IParentsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Get a parent user by Id
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [Route("{parentId:int}", Name = "GetParentById")]
        [HttpGet]
        public IHttpActionResult GetParentById(int parentId)
        {
            return Ok(service.GetParentById(parentId));
        }

        /// <summary>
        /// Return a list of all parents with their children
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllParentsWithChildren()
        {
            return Ok(service.GetAllParentsWithTheirChildrent());
        }

        /// <summary>
        /// Get the children for a given parent
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [Route("{parentId}/children")]
        [HttpGet]
        public IHttpActionResult GetChildrenOfParent(int parentId)
        {
            return Ok(service.GetAllChildren(parentId));
        }

        /// <summary>
        /// Add a student user as a child to the given parent user
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [Route("{parentId}/children")]
        [HttpPost]
        public IHttpActionResult AddStudentAsChild(int parentId, [FromUri]int studentId)
        {
            return Ok(service.AddChild(parentId, studentId));
        } 

        /// <summary>
        /// Get the parents of the given student
        /// NOTE not sure this is a good place
        /// NOTE not even in students. studentDto by default carries the info
        /// </summary>
        /// <returns></returns>
        [Route("parents-for-students")]
        [HttpGet]
        public IHttpActionResult GetParentsForStudents()
        {
            return Ok(service.GetParentsForStudents());
        }

        #region Update & Delete

        /// <summary>
        /// Update the given Parents' data
        /// NOTE move to accounts controller
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{parentId}")]
        [ResponseType(typeof(ParentDto))]
        public IHttpActionResult PutUpdateParent(int parentId, ParentDto parent)
        {
            return Ok(service.UpdateParent(parentId, parent));
        }

        /// <summary>
        /// Delete the given parent user
        /// NOTE move to accounts controller
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
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
