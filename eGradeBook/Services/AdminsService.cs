using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.Admins;
using eGradeBook.Repositories;
using NLog;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with admin users and administrative tasks
    /// </summary>
    public class AdminsService : IAdminsService
    {
        private IUnitOfWork db;
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public AdminsService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

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
            AdminDto admin = db.GradeBookUsersRepository.Get(u => u.Id == adminId).OfType<AdminUser>()
                .Select(a => new AdminDto()
                {
                    Id = a.Id,
                    FullName = a.FirstName + " " + a.LastName
                })
                .FirstOrDefault();

            return admin;
        }

        /// <summary>
        /// Get all admin users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AdminDto> GetAllAdmins()
        {
            return db.AdminsRepository.Get()
                .Select(a => Converters.AdminsConverter.AdminToAdminDto(a));
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