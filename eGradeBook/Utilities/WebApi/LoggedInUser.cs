using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Utilities.WebApi
{
    public class LoggedInUser
    {
        public int? UserId { get; set; }
        public string UserRole { get; set; }
    }
}