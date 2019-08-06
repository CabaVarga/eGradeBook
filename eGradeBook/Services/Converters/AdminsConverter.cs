using eGradeBook.Models;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Admins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Conversions for Admin users
    /// </summary>
    public static class AdminsConverter
    {
        /// <summary>
        /// Convert an admin model to admin dto
        /// </summary>
        /// <param name="admin">An admin (full) model</param>
        /// <returns>Admin Dto object, ready for Json serialization</returns>
        public static AdminDto AdminToAdminDto(AdminUser admin)
        {
            return new AdminDto()
            {
                Id = admin.Id,
                FirstName = admin.FirstName,
                LastName = admin.LastName,
                Gender = admin.Gender,
                Email = admin.Email,
                Phone = admin.PhoneNumber
            };
        }

        /// <summary>
        /// Update full entity from dto before sending to the storage
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dto"></param>
        public static void UpdateAdminsPersonalData(AdminUser user, AdminUpdateDto dto)
        {
            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Gender = dto.Gender;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
        }

        /// <summary>
        /// Convert an admin registration dto to admin user
        /// </summary>
        /// <param name="adminReg"></param>
        /// <returns></returns>
        public static AdminUser AdminRegistrationDtoToAdmin(AdminRegistrationDto adminReg)
        {
            return new AdminUser()
            {
                UserName = adminReg.UserName,
                FirstName = adminReg.FirstName,
                LastName = adminReg.LastName,
                Gender = adminReg.Gender,
                Email = adminReg.Email,
                PhoneNumber = adminReg.PhoneNumber
            };
        }
    }
}