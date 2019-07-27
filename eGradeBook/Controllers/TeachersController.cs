using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    public class TeachersController : ApiController
    {
        private ITeachersService teachersService;

        public TeachersController(ITeachersService service)
        {
            this.teachersService = service;
        }

        public IHttpActionResult GetAllTeachers()
        {
            return Ok(teachersService.GetAllTeachersDtos());
        }
    }
}