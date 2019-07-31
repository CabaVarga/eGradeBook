﻿using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Registration;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Services;
using NLog;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Web api controller for working with User Accounts
    /// </summary>
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        private IUsersService service;
        private ILogger logger;

        // User data
        private bool isAdmin;
        private bool isAuthenticated;
        private string userEmail;
        private string userId;

        /// <summary>
        /// Accounts Controller constructor
        /// </summary>
        /// <param name="userService"></param>
        public AccountsController(IUsersService userService, ILogger logger)
        {
            this.service = userService;
            this.logger = logger;
        }

        /// <summary>
        /// Register a new admin. It can be done only by an admin. 
        /// Successful registration returns response payload describing and linking to the resource.
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [ResponseType(typeof(CreatedResourceDto))]
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

            var createdId = service.GetIdOfUser(userModel.UserName);

            var link = Url.Link("GetAdminById", new { adminId = createdId });

            logger.Info("userid={0} action={1} result={2} status={3}",
                userId, "RegisterAdmin", createdId, "success");

            return CreatedAtRoute("GetAdminById", new { adminId = createdId }, new CreatedResourceDto()
            {
                Id = createdId,
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

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }


        private void FetchUserData()
        {
            isAdmin = RequestContext.Principal.IsInRole("admins");
            isAuthenticated = RequestContext.Principal.Identity.IsAuthenticated;
            userEmail = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == ClaimTypes.Email)?.Value;
            userId = ((ClaimsPrincipal)RequestContext.Principal).FindFirst(x => x.Type == "UserId")?.Value;
        }
    }
}
