using eGradeBook.Models;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Admins;
using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Models.Dtos.Teachers;
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
        Task<AdminDto> RegisterAdmin(AdminRegistrationDto user);

        /// <summary>
        /// Register student
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<StudentDto> RegisterStudent(StudentRegistrationDto user);

        /// <summary>
        /// Register teacher
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<TeacherDto> RegisterTeacher(TeacherRegistrationDto user);

        /// <summary>
        /// Register parent
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ParentDto> RegisterParent(ParentRegistrationDto user);

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

        /// <summary>
        /// Update admin user
        /// NOTE It is maybe better if the update and delete are attached to the endpoints....
        /// NOTE Even the registration is a bit in the wrong place..
        /// </summary>
        /// <param name="adminUpdate"></param>
        /// <returns></returns>
        Task<AdminDto> UpdateAdmin(AdminUpdateDto adminUpdate);
        Task<TeacherDto> UpdateTeacher(TeacherUpdateDto teacherUpdate);
        Task<StudentDto> UpdateStudent(StudentUpdateDto studentUpdate);
        Task<ParentDto> UpdateParent(ParentUpdateDto parentUpdate);
    }
}