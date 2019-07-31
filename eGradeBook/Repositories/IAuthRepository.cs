using eGradeBook.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Repositories
{
    /// <summary>
    /// Interface for Auth repository, the specialized repository for working with authentication data
    /// </summary>
    public interface IAuthRepository : IDisposable
    {
        /// <summary>
        /// Register an admin user
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterAdminUser(AdminUser userModel, string password);

        /// <summary>
        /// Register a student user
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterStudentUser(StudentUser userModel, string password);

        /// <summary>
        /// Register a teacher user
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterTeacherUser(TeacherUser userModel, string password);

        /// <summary>
        /// Register a parent user
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterParentUser(ParentUser userModel, string password);

        /// <summary>
        /// Register a class master user
        /// NOTE: not implemented in services and controllers
        /// </summary>
        /// <param name="userModel"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterClassMasterUser(ClassMasterUser userModel, string password);

        /// <summary>
        /// Find a user by a given username. Password is required.
        /// Used at token fetching endpoint
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<GradeBookUser> FindUser(string userName, string password);

        /// <summary>
        /// Find the roles for the given user.
        /// The current version does not take into account multiple roles for a single user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IList<string>> FindRoles(int userId);
    }
}