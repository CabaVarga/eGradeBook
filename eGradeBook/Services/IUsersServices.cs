using eGradeBook.Models;
using eGradeBook.Models.Dtos.Accounts;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service interface for users management tasks
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Register an admin
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterAdmin(AdminRegistrationDto user);

        /// <summary>
        /// Register student
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterStudent(StudentRegistrationDto user);

        /// <summary>
        /// Register teacher
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterTeacher(TeacherRegistrationDto user);

        /// <summary>
        /// Register parent
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterParent(ParentRegistrationDto user);

        /// <summary>
        /// Register class master
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<IdentityResult> RegisterClassMaster(UserRegistrationDto user);

        /// <summary>
        /// Get an id for a username
        /// NOTE heavily used in Accounts controller
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        int GetIdOfUser(string username);


        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IdentityResult> DeleteUser(int userId);

        /// <summary>
        /// Used by the Who Am I and Who Is in the accounts / users controller
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserDataDto GetUserData(int userId);
    }
}