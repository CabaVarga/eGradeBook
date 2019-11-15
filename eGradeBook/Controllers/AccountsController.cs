using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Admins;
using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.PasswordUpdate;
using eGradeBook.Models.Dtos.Registration;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Services;
using eGradeBook.SwaggerHelpers.Examples;
using eGradeBook.Utilities.Common;
using eGradeBook.Utilities.WebApi;
using NLog;
using Swashbuckle.Examples;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Web api controller for working with User Accounts
    /// </summary>
    [RoutePrefix("api/accounts")]
    // [Authorize]
    public class AccountsController : ApiController
    {
        private IUsersService service;
        private IFileResourcesService fileResourcesService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Accounts Controller constructor
        /// </summary>
        /// <param name="userService"></param>
        public AccountsController(IUsersService userService, IFileResourcesService fileResourcesService)
        {
            this.service = userService;
            this.fileResourcesService = fileResourcesService;
        }

        #region Registrations

        /// <summary>
        /// Check if username is available
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("check-username/{username}")]
        [HttpGet]
        public async Task<IHttpActionResult> CheckUsername(string username)
        {
            var available = await service.CheckUsername(username);

            return Ok(available);
        }

        /// <summary>
        /// Register a new Admin
        /// NOTE I need to return a full AdminDto at creation, the current solution is not good.
        /// </summary>
        /// <param name="userModel">Dto object with account and personal details.</param>
        /// <returns>CreatedResourceDto payload describing and linking to the resource</returns>
        [ResponseType(typeof(AdminDto))]
        // [Authorize(Roles = "admins")]
        [SwaggerRequestExample(typeof(AdminRegistrationDto), typeof(RegisterAdminExample))]
        [Route("register-admin")]
        public async Task<IHttpActionResult> RegisterAdmin(AdminRegistrationDto userModel)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Register Admin {@adminReg} by {@userData}", userModel, userData);

            var result = await service.RegisterAdmin(userModel);

            logger.Info("Created Admin {@userId}", result.AdminId);

            return CreatedAtRoute("GetAdminById", new { adminId = result.AdminId }, result);
        }

        /// <summary>
        /// Register a new Teacher
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns>CreatedResourceDto payload describing and linking to the resource</returns>
        [Route("register-teacher")]
        [Authorize(Roles = "admins")]
        [SwaggerRequestExample(typeof(TeacherRegistrationDto), typeof(RegisterTeacherExample))]
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
        [SwaggerRequestExample(typeof(StudentRegistrationDto), typeof(RegisterStudentExample))]
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
        [SwaggerRequestExample(typeof(ParentRegistrationDto), typeof(RegisterParentExample))]
        public async Task<IHttpActionResult> RegisterParent(ParentRegistrationDto userModel)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Register Parent {@parentReg} by {@userData}", userModel, userData);

            var result = await service.RegisterParent(userModel);

            logger.Info("Created Parent {@userId}", result.ParentId);

            return CreatedAtRoute("GetParentById", new { parentId = result.ParentId }, result);
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
        [Authorize(Roles = "admins,teachers")]
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
        [Authorize(Roles = "admins,students")]
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
        [Authorize(Roles = "admins,parents")]
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
        [Authorize(Roles ="admins, teachers, students, parents")]
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
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetWhoIsUser(int userId)
        {
            return Ok(service.GetUserData(userId));
        }

        #region CRUD
        /// <summary>
        /// Update password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{userId}/changePassword")]
        [ResponseType(typeof(PasswordDto))]
        public async Task<IHttpActionResult> ChangePassword(int userId, PasswordDto passwordDto)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Password change requested for {@userId} by {@userData}", userId, userData);

            var userInfo = Utilities.WebApi.IdentityHelper.FetchUserData(RequestContext);

            if (userInfo.UserId == null)
            {
                return Unauthorized();
            }

            if (userId != passwordDto.UserId)
            {
                return BadRequest();
            }

            var loggedInUserId = userInfo.UserId ?? 0;

            if (userInfo.IsAdmin)
            {
                // an admin can change anything
                //if (loggedInUserId != userId)
                //{
                //    return Unauthorized();
                //}
            }
            else 
            {
                // anyone else -> only can change own data
                if (loggedInUserId != userId)
                {
                    return Unauthorized();
                }
            }

            var result = await service.ChangePassword(userId, passwordDto);

            if (!result)
            {
                return NotFound();
            }

            return Ok(passwordDto);
        }

        /// <summary>
        /// Variation of the exercise from the class.
        /// We need to connect the file with the given user as an owner.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Status code with empty payload.</returns>
        [Route("{userId}/upload")]
        [HttpPost]
        [ResponseType(typeof(FileResourceDto))]
        public async Task<IHttpActionResult> UploadFileForUser(int userId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Password change requested for {@userId} by {@userData}", userId, userData);

            var userInfo = Utilities.WebApi.IdentityHelper.FetchUserData(RequestContext);

            // admin can upload, change and remoce for anyone
            // others only for themselves

            // Check if user exists
            var owner = service.GetUserData(userId);
            int fileId = 0;

            if (owner == null)
            {
                return NotFound();
            }

            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = HttpContext.Current.Server.MapPath("~/App_Data");

            var provider = new MultipartFormDataStreamProvider(root);

            // OK. We will take the file, save it, and return the info about the created resource...
            // Ideally, we would certainly connect with EntityFramework. But in this case, we will only save
            // The id, without real reference...

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);
                // This illustrates how to get the file names.
                foreach (MultipartFileData file in provider.FileData)
                {
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                    Trace.WriteLine("Server file path: " + file.LocalFileName);

                    char[] charsToTrim = { '"' };

                    Trace.WriteLine(file.Headers.ContentDisposition.FileName.Trim(charsToTrim));
                    string fileOriginal = file.Headers.ContentDisposition.FileName.Trim(charsToTrim);

                    // FileResource creation logic comes here: (probably not optimal)
                    FileResource fileResource = new FileResource()
                    {
                        UserId = userId,
                        Description = fileOriginal,
                        Path = file.LocalFileName
                    };

                    // If a user can create a FileResource, should I add that capability to the UsersService?

                    var fr = fileResourcesService.CreateFileResource(fileResource); // probably need method with parameters
                    fileId = fr.Id;
                    //return CreatedAtRoute("FileResource", new { id = fileId }, fr);

                    var frDto = new FileResourceDto
                    {
                        Id = fr.Id,
                        UserId = fr.UserId,
                        Description = fr.Description
                    };

                    // This is for a single file!
                    // for multiple files I need another enpoint
                    return Ok(frDto);
                }
                return BadRequest("There were no files in the request");
            }
            catch (System.Exception e)
            {
                return InternalServerError(e);
            }
        }
        #endregion

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
