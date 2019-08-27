using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Admins;
using eGradeBook.Models.Dtos.ClassMasters;
using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
using eGradeBook.Services.Exceptions;
using eGradeBook.Utilities.Common;
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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public UsersService(IUnitOfWork unitOfWork)
        {
            db = unitOfWork;
        }

        /// <summary>
        /// Register an admin
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<AdminDto> RegisterAdmin(AdminRegistrationDto userModel)
        {
            logger.Trace("layer={0} class={1} method={2} stage={3}", "service", "users", "registerAdmin", "init");

            AdminUser user = Converters.AdminsConverter.AdminRegistrationDtoToAdmin(userModel);

            var result = await db.AuthRepository.RegisterAdminUser(user, userModel.Password);

            if (!result.Succeeded)
            {
                var ex = new UserRegistrationException(result.Errors.ToArray());
                ex.Data.Add("IdentityResultErrors", result.Errors.ToArray());
                throw ex;
            }

            user = await db.AuthRepository.FindUserByUserName(userModel.UserName) as AdminUser;

            return Converters.AdminsConverter.AdminToAdminDto(user);
        }

        /// <summary>
        /// Register a teacher
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<TeacherDto> RegisterTeacher(TeacherRegistrationDto userModel)
        {
            TeacherUser user = TeachersConverter.TeacherRegistrationDtoToTeacher(userModel);

            var result = await db.AuthRepository.RegisterTeacherUser(user, userModel.Password);

            if (!result.Succeeded)
            {
                var ex = new UserRegistrationException(result.Errors.ToArray());
                ex.Data.Add("IdentityResultErrors", result.Errors.ToArray());
                throw ex;
            }

            user = await db.AuthRepository.FindUserByUserName(userModel.UserName) as TeacherUser;

            return TeachersConverter.TeacherToTeacherDto(user);
        }

        /// <summary>
        /// Register a student
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<StudentDto> RegisterStudent(StudentRegistrationDto userModel)
        {
            StudentUser user = StudentsConverter.StudentRegistrationDtoToStudent(userModel);

            var result = await db.AuthRepository.RegisterStudentUser(user, userModel.Password);

            if (!result.Succeeded)
            {
                var ex = new UserRegistrationException(result.Errors.ToArray());
                ex.Data.Add("IdentityResultErrors", result.Errors.ToArray());
                throw ex;
            }

            user = await db.AuthRepository.FindUserByUserName(userModel.UserName) as StudentUser;

            return StudentsConverter.StudentToStudentDto(user);
        }
        /// <summary>
        /// Register a parent
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public async Task<ParentDto> RegisterParent(ParentRegistrationDto userModel)
        {
            ParentUser user = ParentsConverter.ParentRegistrationDtoToParent(userModel);

            var result = await db.AuthRepository.RegisterParentUser(user, userModel.Password);

            if (!result.Succeeded)
            {
                var ex = new UserRegistrationException(result.Errors.ToArray());
                ex.Data.Add("IdentityResultErrors", result.Errors.ToArray());
                throw ex;
            }

            user = await db.AuthRepository.FindUserByUserName(userModel.UserName) as ParentUser;

            return ParentsConverter.ParentToParentDto(user);
        }

        /// <summary>
        /// Update admin
        /// </summary>
        /// <param name="adminUpdate"></param>
        /// <returns></returns>
        public async Task<AdminDto> UpdateAdmin(AdminUpdateDto adminUpdate)
        {
            AdminUser user = db.AdminsRepository.Get(a => a.Id == adminUpdate.Id).FirstOrDefault();

            if (user == null)
            {
                // 404, Not found.
                // No reason for an exception I think
                return null;
            }

            AdminsConverter.UpdateAdminsPersonalData(user, adminUpdate);

            var result = await db.AuthRepository.UpdateUser(user);

            if (!result.Succeeded)
            {
                var ex = new UserUpdateException(result.Errors.ToArray());
                ex.Data.Add("IdentityResultErrors", result.Errors.ToArray());
                throw ex;
            }

            var updatedUser = db.AdminsRepository.Get(a => a.Id == adminUpdate.Id).FirstOrDefault();

            return AdminsConverter.AdminToAdminDto(updatedUser);
        }

        /// <summary>
        /// Update teacher
        /// </summary>
        /// <param name="teacherUpdate"></param>
        /// <returns></returns>
        public async Task<TeacherDto> UpdateTeacher(TeacherUpdateDto teacherUpdate)
        {
            TeacherUser user = db.TeachersRepository.Get(a => a.Id == teacherUpdate.Id).FirstOrDefault();

            if (user == null)
            {
                // 404, Not found.
                // No reason for an exception I think
                return null;
            }

            TeachersConverter.UpdateTeachersPersonalData(user, teacherUpdate);

            var result = await db.AuthRepository.UpdateUser(user);

            if (!result.Succeeded)
            {
                var ex = new UserUpdateException(result.Errors.ToArray());
                ex.Data.Add("IdentityResultErrors", result.Errors.ToArray());
                throw ex;
            }

            var updatedUser = db.TeachersRepository.Get(a => a.Id == teacherUpdate.Id).FirstOrDefault();

            return TeachersConverter.TeacherToTeacherDto(updatedUser);
        }

        /// <summary>
        /// Update student
        /// </summary>
        /// <param name="studentUpdate"></param>
        /// <returns></returns>
        public async Task<StudentDto> UpdateStudent(StudentUpdateDto studentUpdate)
        {
            StudentUser user = db.StudentsRepository.Get(a => a.Id == studentUpdate.Id).FirstOrDefault();

            if (user == null)
            {
                // 404, Not found.
                // No reason for an exception I think
                return null;
            }

            StudentsConverter.UpdateStudentsPersonalData(user, studentUpdate);

            var result = await db.AuthRepository.UpdateUser(user);

            if (!result.Succeeded)
            {
                var ex = new UserUpdateException(result.Errors.ToArray());
                ex.Data.Add("IdentityResultErrors", result.Errors.ToArray());
                throw ex;
            }

            var updatedUser = db.StudentsRepository.Get(a => a.Id == studentUpdate.Id).FirstOrDefault();

            return StudentsConverter.StudentToStudentDto(updatedUser);
        }

        /// <summary>
        /// Update parent
        /// </summary>
        /// <param name="parentUpdate"></param>
        /// <returns></returns>
        public async Task<ParentDto> UpdateParent(ParentUpdateDto parentUpdate)
        {
            ParentUser user = db.ParentsRepository.Get(a => a.Id == parentUpdate.Id).FirstOrDefault();

            if (user == null)
            {
                // 404, Not found.
                // No reason for an exception I think
                return null;
            }

            ParentsConverter.UpdateParentsPersonalData(user, parentUpdate);

            var result = await db.AuthRepository.UpdateUser(user);

            if (!result.Succeeded)
            {
                var ex = new UserUpdateException(result.Errors.ToArray());
                ex.Data.Add("IdentityResultErrors", result.Errors.ToArray());
                throw ex;
            }

            var updatedUser = db.ParentsRepository.Get(a => a.Id == parentUpdate.Id).FirstOrDefault();

            return ParentsConverter.ParentToParentDto(updatedUser);
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IdentityResult> DeleteUser(int userId)
        {
            return await db.AuthRepository.DeleteUser(userId);
        }

        #region Helpers
        /// <summary>
        /// Get an id of a user with a given username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public int GetIdOfUser(string username)
        {
            return db.GradeBookUsersRepository.Get(u => u.UserName == username).Select(u => u.Id).FirstOrDefault();
        }

        /// <summary>
        /// Used by the WhoAmI and WhoIs endpoints
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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

            if (user is AdminUser)
            {
                dataDto.Role = UserRole.ADMIN;
            }
            else if (user is TeacherUser)
            {
                dataDto.Role = UserRole.TEACHER;
            }
            else if (user is StudentUser)
            {
                dataDto.Role = UserRole.STUDENT;
            }
            else if (user is ParentUser)
            {
                dataDto.Role = UserRole.PARENT;
            }

            return dataDto;
        }

        public async Task<bool> CheckUsername(string username)
        {
            var result = await db.AuthRepository.FindUserByUserName(username);

            if (result == null)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}