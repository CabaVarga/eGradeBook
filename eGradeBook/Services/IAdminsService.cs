using eGradeBook.Models.Dtos.Admins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Interface for Admins Service
    /// </summary>
    public interface IAdminsService
    {
        // CRUD without the C

        /// <summary>
        /// Get an admin by Id
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        AdminDto GetAdminById(int adminId);

        /// <summary>
        /// Get all admins
        /// </summary>
        /// <returns></returns>
        IEnumerable<AdminDto> GetAllAdmins();

        /// <summary>
        /// Update an admin
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="admin"></param>
        /// <returns></returns>
        AdminDto UpdateAdmin(int adminId, AdminDto admin);

        /// <summary>
        /// Delete an admin
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        AdminDto DeleteAdmin(int adminId);
    }
}