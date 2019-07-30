using eGradeBook.Models.Dtos.Admins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface IAdminsService
    {
        // CRUD without the C
        AdminDto GetAdminById(int adminId);
        IEnumerable<AdminDto> GetAllAdmins();
        AdminDto UpdateAdmin(int adminId, AdminDto admin);
        AdminDto DeleteAdmin(int adminId);
    }
}