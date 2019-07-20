using eGradeBook.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Repositories
{
    public interface IAuthRepository : IDisposable
    {
        Task<IdentityResult> RegisterAdminUser(AdminUser userModel, string password);
        Task<IdentityResult> RegisterStudentUser(StudentUser userModel, string password);
        Task<IdentityResult> RegisterTeacherUser(TeacherUser userModel, string password);
        Task<IdentityResult> RegisterParentUser(ParentUser userModel, string password);
        Task<IdentityResult> RegisterClassMasterUser(ClassMasterUser userModel, string password);
        Task<GradeBookUser> FindUser(string userName, string password);
        Task<IList<string>> FindRoles(int userId);
    }
}