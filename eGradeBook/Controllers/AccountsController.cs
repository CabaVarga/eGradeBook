using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Registration;
using eGradeBook.Models.Dtos.Students;
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

        /// <summary>
        /// Register a new admin. It can be done only by an admin. 
        /// Successful registration returns response payload describing and linking to the resource.
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns></returns>
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

            var userId = service.GetIdOfUser(userModel.UserName);

            var link = Url.Link("GetAdminById", new { adminId = userId });

            return CreatedAtRoute("GetAdminById", new { adminId = userId }, new CreatedResourceDto()
            {
                Id = userId,
                Location = link,
                Type = UserType.ADMIN
            });
        }

        /// <summary>
        /// Register a new admin. It can be done only by an admin. 
        /// Successful registration returns response payload describing and linking to the resource.
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("register-teacher")]
        public async Task<IHttpActionResult> RegisterTeacher(UserDTO userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.RegisterTeacher(userModel);

            if (result == null)
            {
                return BadRequest(ModelState);
            }

            var userId = service.GetIdOfUser(userModel.UserName);

            var link = Url.Link("GetTeacherById", new { teacherId = userId });

            return CreatedAtRoute("GetTeacherById", new { teacherId = userId }, new CreatedResourceDto()
            {
                Id = userId,
                Location = link,
                Type = UserType.TEACHER
            });
        }

        /// <summary>
        /// Register a new student. It can be done only by an admin. 
        /// Successful registration returns response payload describing and linking to the resource.
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns></returns>
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

            var userId = service.GetIdOfUser(userModel.UserName);

            var link = Url.Link("GetStudentById", new { studentId = userId });

            return CreatedAtRoute("GetStudentById", new { studentId = userId },  new CreatedResourceDto()
            {
                Id = userId,
                Location = link,
                Type = UserType.STUDENT
            });
        }
        /// <summary>
        /// Register a new parent. It can be done only by an admin. 
        /// Successful registration returns response payload describing and linking to the resource.
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("register-parent")]
        public async Task<IHttpActionResult> RegisterParent(UserDTO userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.RegisterParent(userModel);

            if (result == null)
            {
                return BadRequest(ModelState);
            }

            var userId = service.GetIdOfUser(userModel.UserName);

            var link = Url.Link("GetParentById", new { parentId = userId });

            return CreatedAtRoute("GetParentById", new { parentId = userId }, new CreatedResourceDto()
            {
                Id = userId,
                Location = link,
                Type = UserType.PARENT
            });
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

    }
}
