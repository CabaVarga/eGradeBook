using eGradeBook.Models.Dtos;
using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        private IUsersService service;

        public AccountsController(IUsersService userService)
        {
            this.service = userService;
        }

        [AllowAnonymous]
        [Route("register-student")]
        public async Task<IHttpActionResult> RegisterStudent(UserDTO userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.RegisterStudent(userModel);

            if (result == null)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [AllowAnonymous]
        [Route("register-admin")]
        public async Task<IHttpActionResult> RegisterAdmin(UserDTO userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.RegisterAdmin(userModel);

            if (result == null)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
