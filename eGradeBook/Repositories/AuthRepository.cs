using eGradeBook.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
            var result = await _userManager.CreateAsync(admin, password);
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
            var result = await _userManager.CreateAsync(student, password);
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
            var result = await _userManager.CreateAsync(teacher, password);
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
            var result = await _userManager.CreateAsync(parent, password);
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
            GradeBookUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        /// <summary>
        /// Find roles for a given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IList<string>> FindRoles(int userId)
        {
            return await _userManager.GetRolesAsync(userId);
        }
        #endregion

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