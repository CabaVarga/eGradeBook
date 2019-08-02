using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Registration;
using eGradeBook.Services;
using eGradeBook.Utilities.StructuredLogging;
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
        /// <param name="logger"></param>
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
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(CreatedResourceDto))]
        [Route("register-admin")]
        public async Task<IHttpActionResult> RegisterAdmin(AdminRegistrationDto userModel)
        {
            logger.Trace("layer={0} class={1} method={2} stage={3}", "api", "accounts", "registerAdmin", "init");
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
        /// Register a new teacher. It can be done only by an admin. 
        /// Successful registration returns response payload describing and linking to the resource.
        /// </summary>
        /// <param name="userModel">Dto object with username, first and last names and password.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("register-teacher")]
        public async Task<IHttpActionResult> RegisterTeacher(TeacherRegistrationDto userModel)
        {
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
        public async Task<IHttpActionResult> RegisterStudent(StudentRegistrationDto userModel)
        {
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
        public async Task<IHttpActionResult> RegisterParent(ParentRegistrationDto userModel)
        {
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

        /// <summary>
        /// Delete user
        /// NOTE Probably need different endpoints for different types of users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult DeleteUser(int id)
        {
            return Ok(service.DeleteUser(id));
        }

        /// <summary>
        /// Retrieve info about the logged in user
        /// </summary>
        /// <returns></returns>
        [Route("whoami")]
        [ResponseType(typeof(UserDataDto))]
        [HttpGet]
        public IHttpActionResult GetWhoAmI()
        {
            FetchUserData();
            return Ok(service.GetUserData(int.Parse(userId)));
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
