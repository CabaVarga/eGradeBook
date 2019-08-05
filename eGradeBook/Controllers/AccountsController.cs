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
        private ILogger logger;

        /// <summary>
        /// Accounts Controller constructor
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="logger"></param>
        public AccountsController(IUsersService userService, ILogger logger)
        {
            this.service = userService;
            this.logger = logger;
        }

        #region Registrations

        /// <summary>
        /// Register a new Admin
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns>CreatedResourceDto payload describing and linking to the resource</returns>
        [ResponseType(typeof(CreatedResourceDto))]
        [Authorize(Roles = "admins")]
        [Route("register-admin")]
        public async Task<IHttpActionResult> RegisterAdmin(AdminRegistrationDto userModel)
        {
            logger.Trace("Registration of new Admin user {@adminuser} initiated", userModel);

            var whereAmI = new Where()
            {
                Layer = "Web api",
                NameSpace = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace,
                ClassMethod = System.Reflection.MethodBase.GetCurrentMethod().Name,
                ClassName = GetType().Name
            };

            logger.Trace("At {@whereAmI}", whereAmI);

            // ("At {@where} user {@who} does {@what} with {@input}
            // ("Who {@user} When {@timestamp} Where {@context} What {@command} Result{@exception}")

            var result = await service.RegisterAdmin(userModel);

            if (result == null)
            {
                // Model state validation already went into validator, something else is the problem
                return BadRequest(ModelState);
            }

            var createdId = service.GetIdOfUser(userModel.UserName);

            var link = Url.Link("GetAdminById", new { adminId = createdId });

            // If I want to get a user dto i need to make some changes
            var data = IdentityHelper.FetchUserData(RequestContext);

            logger.Info("userid={0} action={1} result={2} status={3}",
                data.UserId, "RegisterAdmin", createdId, "success");

            return CreatedAtRoute("GetAdminById", new { adminId = createdId }, new CreatedResourceDto()
            {
                Id = createdId,
                Location = link,
                UserRole = UserRole.ADMIN
            });
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

            logger.Trace("Registration of new Teacher user {@teacheruser} by {@loggedUser}", userModel, userData);

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
                UserRole = UserRole.TEACHER
            });
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
            var result = await service.RegisterStudent(userModel);

            if (result == null)
            {
                return BadRequest(ModelState);
            }

            // Had to change auth repo implementation
            if (!result.Succeeded)
            {
                logger.Error("Student registration failed {errors}", result.Errors);
            }

            var userId = service.GetIdOfUser(userModel.UserName);

            var link = Url.Link("GetStudentById", new { studentId = userId });

            return CreatedAtRoute("GetStudentById", new { studentId = userId }, new CreatedResourceDto()
            {
                Id = userId,
                Location = link,
                UserRole = UserRole.STUDENT
            });
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
            // Authentication and authorization data about logged in user
            var user = IdentityHelper.FetchUserData(RequestContext);

            logger.Info("User {@userData} is attempting to register a parent {@parent}", user, userModel);

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
                UserRole = UserRole.PARENT
            });
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
            var user = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("User {@userData} requesting an Admin {adminId} update {@adminData}", user, adminId, adminUpdate);

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
            var user = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("User {@userData} requesting a Teacher {teacherId} update {@teacherData}", user, teacherId, teacherUpdate);

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
            var user = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("User {@userData} requesting a Student {studentId} update {@studentData}", user, studentId, studentUpdate);

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
            var user = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("User {@userData} requesting a Parent {parentId} update {@parentData}", user, parentId, parentUpdate);

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
            var currentUser = IdentityHelper.FetchUserData(RequestContext);
            var current = IdentityHelper.GetLoggedInUser(currentUser);

            logger.Trace("Removal of user {userId} initiated by {@loggedUser}", id, current);

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
