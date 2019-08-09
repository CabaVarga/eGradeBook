using eGradeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface IEmailsService
    {
        void NotifyParents(Grade grade);
    }
}