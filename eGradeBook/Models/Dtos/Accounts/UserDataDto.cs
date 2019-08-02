using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Models.Dtos.Accounts
{
    public class UserDataDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        ADMIN,
        TEACHER,
        STUDENT,
        PARENT
    }
}