using eGradeBook.Models;
using eGradeBook.Models.Dtos.PasswordUpdate;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Repositories
{
    /// <summary>
    /// Auth repository is a special kind of repository for working with Identity data through the UserManager and UserStore objects.
    /// </summary>
    public class AuthRepository : IAuthRepository, IDisposable
    {
        private UserManager<GradeBookUser, int> _userManager;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor, taking an existing DbContext derived object
        /// </summary>
        /// <param name="context"></param>
        public AuthRepository(DbContext context)
        {
            _userManager = new UserManager<GradeBookUser, int>(
                new UserStore<GradeBookUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>(context));
        }

        #region Registrations
        /// <summary>
        /// Register an admin user
        /// </summary>
        /// <param name="admin"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterAdminUser(AdminUser admin, string password)
        {
            logger.Trace("Register Admin {@userName}", admin.UserName);
            
            var result = await _userManager.CreateAsync(admin, password);

            if (!result.Succeeded)
            {
                return result;
            }

            _userManager.AddToRole(admin.Id, "admins");
            return result;
        }

        /// <summary>
        /// Register a student user
        /// </summary>
        /// <param name="student"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterStudentUser(StudentUser student, string password)
        {
            logger.Trace("Register Student {@userName}", student.UserName);
            
            var result = await _userManager.CreateAsync(student, password);

            if (!result.Succeeded)
            {
                return result;
            }

            // Two different tasks. Further logic required because I cant just send back not created!
            _userManager.AddToRole(student.Id, "students");

            return result;
        }

        /// <summary>
        /// Register a teacher user
        /// </summary>
        /// <param name="teacher"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterTeacherUser(TeacherUser teacher, string password)
        {
            logger.Trace("Register Teacher {@userName}", teacher.UserName);

            var result = await _userManager.CreateAsync(teacher, password);

            if (!result.Succeeded)
            {
                return result;
            }

            _userManager.AddToRole(teacher.Id, "teachers");
            return result;
        }

        /// <summary>
        /// Register a parent user (used from the accounts controller)
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterParentUser(ParentUser parent, string password)
        {
            logger.Trace("Register Parent {@userName}", parent.UserName);

            var result = await _userManager.CreateAsync(parent, password);

            if (!result.Succeeded)
            {
                return result;
            }

            _userManager.AddToRole(parent.Id, "parents");
            return result;
        }

        /// <summary>
        /// Register a ClassMaster user (not in use currently)
        /// </summary>
        /// <param name="classMaster"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RegisterClassMasterUser(ClassMasterUser classMaster, string password)
        {
            var result = await _userManager.CreateAsync(classMaster, password);
            _userManager.AddToRole(classMaster.Id, "classmasters");
            return result;
        }
        #endregion

        #region Find user and find roles
        /// <summary>
        /// Find user for provided username AND password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<GradeBookUser> FindUser(string userName, string password)
        {
            logger.Trace("Find User {@userName}", userName);

            GradeBookUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        /// <summary>
        /// Find user by provided username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<GradeBookUser> FindUserByUserName(string userName)
        {
            logger.Trace("Find User {@userName}", userName);

            GradeBookUser user = await _userManager.FindByNameAsync(userName);

            return user;
        }

        /// <summary>
        /// Find roles for a given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IList<string>> FindRoles(int userId)
        {
            logger.Trace("Find Roles for {@userId}}", userId);
            var result = await _userManager.GetRolesAsync(userId);
            return result;
        }
        #endregion

        #region Update users
        /// <summary>
        /// Common user update method,
        /// will not work for special properties!
        /// It IS working, after all.
        /// Only you need special Dto for controller and service, to update the special properties!
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IdentityResult> UpdateUser(GradeBookUser user)
        {
            // NOTE this will not work for concrete users
            // It will only update the common properties, not the special ones....
            // UPDATE It is working, indeed
            var result = await _userManager.UpdateAsync(user);

            return result;
        }

        /// <summary>
        /// Change password for user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passwordDto"></param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangePassword(int userId, PasswordDto passwordDto)
        {
            // NOTE this will not work for concrete users
            // It will only update the common properties, not the special ones....
            // UPDATE It is working, indeed
            var result = await _userManager.ChangePasswordAsync(userId, passwordDto.CurrentPassword, passwordDto.NewPassword);

            return result;
        }

        #endregion

        /// <summary>
        /// Delete a user from the system
        /// </summary>
        /// <param name="userId">The user id of the user we want to delete</param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteUser(int userId)
        {
            logger.Trace("AuthRepository delete user {userId}", userId);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                logger.Info("User {userId} not found and could not be deleted", userId);
                var result = new IdentityResult(new string[] { "User not found" });
                return result;
            }

            var roles = await _userManager.GetRolesAsync(user.Id);

            var removedFromRoles = await _userManager.RemoveFromRolesAsync(user.Id, roles.ToArray());
            
            if (!removedFromRoles.Succeeded)
            {
                // Removal unsuccessful
                // Log and return
                logger.Info("Could not remove user {userId} from roles {roles}", userId, roles);
                return removedFromRoles;
            }

            var deleted = await _userManager.DeleteAsync(user);

            return deleted;
        }

        /// <summary>
        /// Hand written Dispose method
        /// </summary>
        public void Dispose()
        {
            if (_userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }
        }
    }
}