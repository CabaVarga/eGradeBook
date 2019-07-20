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
    public class AuthRepository : IAuthRepository, IDisposable
    {
        private UserManager<GradeBookUser, int> _userManager;

        public AuthRepository(DbContext context)
        {
            _userManager = new UserManager<GradeBookUser, int>(
                new UserStore<GradeBookUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>(context));
        }

        #region Registrations
        public async Task<IdentityResult> RegisterAdminUser(AdminUser admin, string password)
        {
            var result = await _userManager.CreateAsync(admin, password);
            _userManager.AddToRole(admin.Id, "admins");
            return result;
        }

        public async Task<IdentityResult> RegisterStudentUser(StudentUser student, string password)
        {
            var result = await _userManager.CreateAsync(student, password);
            _userManager.AddToRole(student.Id, "students");
            return result;
        }

        public async Task<IdentityResult> RegisterTeacherUser(TeacherUser teacher, string password)
        {
            var result = await _userManager.CreateAsync(teacher, password);
            _userManager.AddToRole(teacher.Id, "teachers");
            return result;
        }

        public async Task<IdentityResult> RegisterParentUser(ParentUser parent, string password)
        {
            var result = await _userManager.CreateAsync(parent, password);
            _userManager.AddToRole(parent.Id, "parents");
            return result;
        }

        public async Task<IdentityResult> RegisterClassMasterUser(ClassMasterUser classMaster, string password)
        {
            var result = await _userManager.CreateAsync(classMaster, password);
            _userManager.AddToRole(classMaster.Id, "classmasters");
            return result;
        }
        #endregion

        #region Find user and find roles
        public async Task<GradeBookUser> FindUser(string userName, string password)
        {
            GradeBookUser user = await _userManager.FindAsync(userName, password);
            return user;
        }

        public async Task<IList<string>> FindRoles(int userId)
        {
            return await _userManager.GetRolesAsync(userId);
        }
        #endregion
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