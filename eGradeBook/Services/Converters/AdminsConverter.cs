using eGradeBook.Models;
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
                FullName = admin.FirstName + " " + admin.LastName
            };
        }
    }
}