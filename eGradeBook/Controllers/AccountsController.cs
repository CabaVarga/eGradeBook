using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Admins;
using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.Registration;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Services;
using eGradeBook.Utilities.Common;
using eGradeBook.Utilities.StructuredLogging;
using eGradeBook.Utilities.WebApi;
using NLog;
using System;
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
    [Authorize]
    public class AccountsController : ApiController
    {
        private IUsersService service;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Accounts Controller constructor
        /// </summary>
        /// <param name="userService"></param>
        public AccountsController(IUsersService userService)
        {
            this.service = userService;
        }

        #region Registrations

        /// <summary>
        /// Register a new Admin
        /// NOTE I need to return a full AdminDto at creation, the current solution is not good.
        /// </summary>
        /// <param name="userModel">Dto object with account and personal details.</param>
        /// <returns>CreatedResourceDto payload describing and linking to the resource</returns>
        [ResponseType(typeof(AdminDto))]
        [Authorize(Roles = "admins")]
        [Route("register-admin")]
        public async Task<IHttpActionResult> RegisterAdmin(AdminRegistrationDto userModel)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Register Admin {@adminReg} by {@userData}", userModel, userData);

            var result = await service.RegisterAdmin(userModel);

            logger.Info("Created Admin {@userId}", result.Id);

            return CreatedAtRoute("GetAdminById", new { adminId = result.Id }, result);
        }

        /// <summary>
        /// Register a new Teacher
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns>CreatedResourceDto payload describing and linking to the resource</returns>
        [Route("register-teacher")]
        [Authorize(Roles = "admins")]
        public async Task<IHttpActionResult> RegisterTeacher(TeacherRegistrationDto userModel)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Trace("Register Teacher {@teacherReg} by {@userData}", userModel, userData);

            var result = await service.RegisterTeacher(userModel);

            logger.Info("Created Teacher {@userId}", result.TeacherId);

            return CreatedAtRoute("GetTeacherById", new { teacherId = result.TeacherId }, result);
        }

        /// <summary>
        /// Register a new Student
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns>CreatedResourceDto payload describing and linking to the resource</returns>
        [Route("register-student")]
        [Authorize(Roles = "admins")]
        public async Task<IHttpActionResult> RegisterStudent(StudentRegistrationDto userModel)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Register Student {@studentReg} by {@userData}", userModel, userData);

            var result = await service.RegisterStudent(userModel);

            logger.Info("Created Student {@userId}", result.StudentId);

            return CreatedAtRoute("GetStudentById", new { studentId = result.StudentId }, result);
        }

        /// <summary>
        /// Register a new Parent
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns>CreatedResourceDto payload describing and linking to the resource</returns>
        [Route("register-parent")]
        [Authorize(Roles = "admins")]
        public async Task<IHttpActionResult> RegisterParent(ParentRegistrationDto userModel)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Register Parent {@parentReg} by {@userData}", userModel, userData);

            var result = await service.RegisterParent(userModel);

            logger.Info("Created Parent {@userId}", result.Id);

            return CreatedAtRoute("GetParentById", new { parentId = result.Id }, result);
        }

        #endregion

        #region Updates
        /// <summary>
        /// Update Admin user
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="adminUpdate"></param>
        /// <returns></returns>
        [Route("update-admin/{adminId}")]
        [HttpPut]
        [ResponseType(typeof(AdminDto))]
        [Authorize(Roles = "admins")]
        public async Task<IHttpActionResult> UpdateAdmin([FromUri] int adminId, [FromBody] AdminUpdateDto adminUpdate)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Update Admin {@adminReg} by {@userData}", adminUpdate, userData);

            if (adminId != adminUpdate.Id)
            {
                logger.Info("Provided Ids do not match");
                return BadRequest("Ids do not match");
            }

            AdminDto updatedAdmin = await service.UpdateAdmin(adminUpdate);

            if (updatedAdmin == null)
            {
                return NotFound();
            }

            return Ok(updatedAdmin);
        }

        /// <summary>
        /// Update Teacher user
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="teacherUpdate"></param>
        /// <returns></returns>
        [Route("update-teacher/{teacherId}")]
        [HttpPut]
        [ResponseType(typeof(TeacherDto))]
        [Authorize(Roles = "admins")]
        public async Task<IHttpActionResult> UpdateTeacher([FromUri] int teacherId, [FromBody] TeacherUpdateDto teacherUpdate)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Update Teacher {@teacherReg} by {@userData}", teacherUpdate, userData);

            if (teacherId != teacherUpdate.Id)
            {
                logger.Info("Provided Ids do not match");
                return BadRequest("Ids do not match");
            }

            var updatedTeacher = await service.UpdateTeacher(teacherUpdate);

            if (updatedTeacher == null)
            {
                return NotFound();
            }

            return Ok(updatedTeacher);
        }

        /// <summary>
        /// Update Student user
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="studentUpdate"></param>
        /// <returns></returns>
        [Route("update-student/{studentId}")]
        [HttpPut]
        [ResponseType(typeof(StudentDto))]
        [Authorize(Roles = "admins")]
        public async Task<IHttpActionResult> UpdateStudent([FromUri] int studentId, [FromBody] StudentUpdateDto studentUpdate)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Update Student {@studentReg} by {@userData}", studentUpdate, userData);

            if (studentId != studentUpdate.Id)
            {
                logger.Info("Provided Ids do not match");
                return BadRequest("Ids do not match");
            }

            StudentDto updatedStudent = await service.UpdateStudent(studentUpdate);

            if (updatedStudent == null)
            {
                return NotFound();
            }

            return Ok(updatedStudent);
        }


        /// <summary>
        /// Update Parent user
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="parentUpdate"></param>
        /// <returns></returns>
        [Route("update-parent/{parentId}")]
        [HttpPut]
        [ResponseType(typeof(ParentDto))]
        [Authorize(Roles = "admins")]
        public async Task<IHttpActionResult> UpdateParent([FromUri] int parentId, [FromBody] ParentUpdateDto parentUpdate)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Update Parent {@parentReg} by {@userData}", parentUpdate, userData);

            if (parentId != parentUpdate.Id)
            {
                logger.Info("Provided Ids do not match");
                return BadRequest("Ids do not match");
            }

            ParentDto updatedParent = await service.UpdateParent(parentUpdate);

            // TODO expand
            return Ok(updatedParent);
        }

        #endregion

        /// <summary>
        /// Delete user
        /// NOTE Probably need different endpoints for different types of users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "admins")]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Trace("Delete User {@userId} by {@userData}", id, userData);

            var result = await service.DeleteUser(id);

            if (result == null)
            {
                return BadRequest(ModelState);
            }

            // Had to change auth repo implementation
            if (!result.Succeeded)
            {
                logger.Error("User removal failed {errors}", result.Errors);
                return InternalServerError();
            }

            return Ok();
        }

        /// <summary>
        /// Retrieve info about the logged in user
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles ="admins,teachers,students,parents")]
        [Route("whoami")]
        [ResponseType(typeof(UserDataDto))]
        [HttpGet]
        public IHttpActionResult GetWhoAmI()
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);

            if (userData.UserId == null)
            {
                return NotFound();
            }

            int myId = (int)userData.UserId;

            return Ok(service.GetUserData(myId));
        }

        /// <summary>
        /// Return basic identifying data about a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("whois/{userId}")]
        [ResponseType(typeof(UserDataDto))]
        [HttpGet]
        [Authorize(Roles = "admins,teachers,students,parents")]
        public IHttpActionResult GetWhoIsUser(int userId)
        {
            return Ok(service.GetUserData(userId));
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
