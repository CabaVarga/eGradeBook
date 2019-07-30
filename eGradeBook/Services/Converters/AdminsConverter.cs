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
        /// <returns>Parent Dto object, ready for Json serialization</returns>
        public static AdminDto ParentToParentDto(AdminUser admin)
        {
            return new AdminDto()
            {
                Id = admin.Id,
                FullName = admin.FirstName + " " + admin.LastName
            };
        }
    }
}