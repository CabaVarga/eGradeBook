using eGradeBook.Models.Dtos.StudentParents;
using eGradeBook.Services;
using eGradeBook.Utilities.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/studentparents")]
    public class StudentParentsController : ApiController
    {
        private IStudentParentsService studentParentsService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public StudentParentsController(IStudentParentsService studentParentsService)
        {
            this.studentParentsService = studentParentsService;
        }

        [Route("{studentParentId}", Name = "GetStudentParent")]
        [HttpGet]
        public IHttpActionResult GetStudentParentById(int studentParentId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get StudentParent {@studentParentId} by {@userData}", studentParentId, userData);

            return Ok(studentParentsService.GetStudentParentDto(studentParentId));
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllStudentParents()
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get all StudentParents by {@userData}", userData);

            return Ok(studentParentsService.GetAllStudentParentsDto());
        }

        [Route("")]
        [HttpPost]
        public IHttpActionResult PostCreateStudentParent(StudentParentDto studentParentDto)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create StudentParent {@studentParentDto} by {@userData}", studentParentDto, userData);

            var result = studentParentsService.CreateStudentParentDto(studentParentDto);

            logger.Info("Created StudentParent {@StudentParentId}", result.StudentParentId);

            return CreatedAtRoute("GetStudentParent", new { studentParentId = result.StudentParentId }, result);
        }
    }
}
