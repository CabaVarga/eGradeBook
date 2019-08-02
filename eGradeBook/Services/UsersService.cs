using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Repositories;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for user accounts management
    /// </summary>
    public class UsersService : IUsersService
    {
        private IUnitOfWork db;
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UsersService(IUnitOfWork unitOfWork, ILogger logger)
        {
            db = unitOfWork;
            this.logger = logger;
        }

        // DELETE START

        /// <summary>
        /// Temporary method for functionality testing
        /// </summary>
        public void teacherclassroom()
        {
            TeacherUser teacher = new TeacherUser();
            ClassRoom schoolClass = new ClassRoom();

            // get to all classRooms!

            var clsrms = teacher.Teachings
                .SelectMany(t => t.Programs)
                .Select(g => g.SchoolClass.Name)
                .Distinct();

            // Piece of cake...

            var stdtns = teacher.Teachings
                .SelectMany(t => t.Programs)
                .SelectMany(g => g.Students)
                .Select(s => s.UserName)
                .Distinct();

            // From classroom:

            var tchrs = schoolClass.Students.SelectMany(s => s.Advancements)
                .Select(t => t.Program)
                .Select(g => g.Teaching)
                .Select(tc => tc.Teacher)
                .Distinct();
        }


        // DELETE END

        /// <summary>
        /// Register an admin
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterAdmin(AdminRegistrationDto userModel)
        {
            logger.Trace("layer={0} class={1} method={2} stage={3}", "service", "users", "registerAdmin", "init");
            AdminUser user = new AdminUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return await db.AuthRepository.RegisterAdminUser(user, userModel.Password);
        }

        /// <summary>
        /// Register a student
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterStudent(StudentRegistrationDto userModel)
        {
            StudentUser user = new StudentUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return await db.AuthRepository.RegisterStudentUser(user, userModel.Password);
        }

        /// <summary>
        /// Register a teacher
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterTeacher(TeacherRegistrationDto userModel)
        {
            TeacherUser user = new TeacherUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return await db.AuthRepository.RegisterTeacherUser(user, userModel.Password);
        }

        /// <summary>
        /// Register a parent
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterParent(ParentRegistrationDto userModel)
        {
            ParentUser user = new ParentUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return await db.AuthRepository.RegisterParentUser(user, userModel.Password);
        }

        /// <summary>
        /// Register a class master
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterClassMaster(UserRegistrationDto userModel)
        {
            ClassMasterUser user = new ClassMasterUser
            {
                UserName = userModel.UserName,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            return await db.AuthRepository.RegisterClassMasterUser(user, userModel.Password);
        }

        public GradeBookUser DeleteUser(int userId)
        {
            GradeBookUser user = db.GradeBookUsersRepository.Get(u => u.Id == userId).FirstOrDefault();

            if (user != null)
            {
                var deleted = db.AuthRepository.DeleteUser(user);
                db.Save();
            }

            return user;
        }

        /// <summary>
        /// Get an id of a user with a given username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetIdOfUser(string username)
        {
            return db.GradeBookUsersRepository.Get(u => u.UserName == username).Select(u => u.Id).FirstOrDefault();
        }

        public UserDataDto GetUserData(int userId)
        {
            GradeBookUser user = db.GradeBookUsersRepository.GetByID(userId);

            if (user == null)
            {
                return null;
            }

            UserDataDto dataDto = new UserDataDto()
            {
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            if (user.GetType() == typeof(AdminUser))
            {
                dataDto.Role = Role.ADMIN;
            }
            else if (user.GetType() == typeof(TeacherUser))
            {
                dataDto.Role = Role.TEACHER;
            }
            else if (user.GetType() == typeof(StudentUser))
            {
                dataDto.Role = Role.STUDENT;
            }
            else
            {
                dataDto.Role = Role.PARENT;
            }

            return dataDto;
        }
    }
}