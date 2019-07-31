using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models.Dtos.Admins;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with admin users and administrative tasks
    /// </summary>
    public class AdminsService : IAdminsService
    {
        /// <summary>
        /// Delete an admin user
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public AdminDto DeleteAdmin(int adminId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get an admin user by id
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public AdminDto GetAdminById(int adminId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get all admin users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdminDto> GetAllAdmins()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update an admin user
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="admin"></param>
        /// <returns></returns>
        public AdminDto UpdateAdmin(int adminId, AdminDto admin)
        {
            throw new NotImplementedException();
        }
    }
}