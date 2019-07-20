using eGradeBook.Models.Dtos;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Services
{
    public interface IUsersService
    {
        Task<IdentityResult> RegisterAdmin(UserDTO user);
        Task<IdentityResult> RegisterStudent(UserDTO user);
        Task<IdentityResult> RegisterTeacher(UserDTO user);
        Task<IdentityResult> RegisterParent(UserDTO user);
        Task<IdentityResult> RegisterClassMaster(UserDTO user);
    }
}