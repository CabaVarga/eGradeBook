using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.Admins;
using eGradeBook.Repositories;
using eGradeBook.Services.Exceptions;
using NLog;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with admin users and administrative tasks
    /// </summary>
    public class AdminsService : IAdminsService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public AdminsService(IUnitOfWork db)
        {
            this.db = db;
        }

        /// <summary>
        /// Delete an admin user
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public async Task<AdminDto> DeleteAdmin(int adminId)
        {
            logger.Info("Service received request for deleting an admin {adminId}", adminId);

            var deletedAdmin = db.AdminsRepository.Get(a => a.Id == adminId).FirstOrDefault();

            if (deletedAdmin == null)
            {
                return null;
            }

            var result = await db.AuthRepository.DeleteUser(adminId);

            if (!result.Succeeded)
            {
                logger.Error("Admin removal failed {errors}", result.Errors);
                //return null;
                throw new ConflictException("Delete admin failed in auth repo");
            }

            return Converters.AdminsConverter.AdminToAdminDto(deletedAdmin);
        }

        /// <summary>
        /// Get an admin user by id
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public AdminDto GetAdminById(int adminId)
        {
            AdminDto admin = db.GradeBookUsersRepository.Get(u => u.Id == adminId).OfType<AdminUser>()
                .Select(a => Converters.AdminsConverter.AdminToAdminDto(a)).FirstOrDefault();

            if (admin == null)
            {
                logger.Info("Admin {@adminId} not found", adminId);
                var ex = new AdminNotFoundException(string.Format("Admin {0} not found", adminId));
                ex.Data.Add("adminId", adminId);
                throw ex;
            }

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