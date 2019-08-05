using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Admins;
using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Repositories;
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
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="logger"></param>
        public UsersService(IUnitOfWork unitOfWork, ILogger logger)
        {
            db = unitOfWork;
            this.logger = logger;
        }

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

            // Instead of directly returning, process the IdentityResult value here
            // Return AdminDto if successfull, not an IdentityResult...
            // So no 201 but 200 if Ok...
            var result = await db.AuthRepository.RegisterAdminUser(user, userModel.Password);

            if (!result.Succeeded)
            {
                var ex = new UserRegistrationException(result.Errors.ToArray());
                ex.Data.Add("IdentityResultErrors", result.Errors.ToArray());
                throw ex;
            }

            return null;
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
                LastName = userModel.LastName,
                PlaceOfBirth = userModel.PlaceOfBirth,
                DateOfBirth = userModel.DateOfBirth
            };

            return await db.AuthRepository.RegisterStudentUser(user, userModel.Password);
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

        /// <summary>
        /// Update admin
        /// </summary>
        /// <param name="adminUpdate"></param>
        /// <returns></returns>
        public async Task<AdminDto> UpdateAdmin(AdminUpdateDto adminUpdate)
        {
            // NOTE big problem
            // Identity is accepting a GradeBookUser.
            // Lets try giving a AdminUser....
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update teacher
        /// </summary>
        /// <param name="teacherUpdate"></param>
        /// <returns></returns>
        public async Task<TeacherDto> UpdateTeacher(TeacherUpdateDto teacherUpdate)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update student
        /// </summary>
        /// <param name="studentUpdate"></param>
        /// <returns></returns>
        public async Task<StudentDto> UpdateStudent(StudentUpdateDto studentUpdate)
        {
            // I will probably have to fetch the user,
            // update some fields
            // send it back to identity...

            var student = db.StudentsRepository.Get(s => s.Id == studentUpdate.Id).FirstOrDefault();

            if (student == null)
            {
                return null;
            }

            student.UserName = studentUpdate.UserName;
            student.FirstName = studentUpdate.FirstName;
            student.LastName = studentUpdate.LastName;
            student.PlaceOfBirth = studentUpdate.PlaceOfBirth;
            student.DateOfBirth = studentUpdate.DateOfBirth;

            var result = await db.AuthRepository.UpdateUser(student);

            if (!result.Succeeded)
            {
                return null;
            }

            var updatedStudent = await db.AuthRepository.FindUserByUserName(studentUpdate.UserName);

            return Converters.StudentsConverter.StudentToStudentDto(updatedStudent as StudentUser);

        }

        /// <summary>
        /// Update parent
        /// </summary>
        /// <param name="parentUpdate"></param>
        /// <returns></returns>
        public async Task<ParentDto> UpdateParent(ParentUpdateDto parentUpdate)
        {
            throw new NotImplementedException();
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
        #endregion
    }
}