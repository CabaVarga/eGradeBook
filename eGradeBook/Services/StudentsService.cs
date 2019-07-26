using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Repositories;

namespace eGradeBook.Services
{
    public class StudentsService : IStudentsService
    {
        private IUnitOfWork db;

        public StudentsService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<StudentUser> GetStudentsByNameStartingWith(string start)
        {
            // as a query i'm not sure it will accept the tolower stuff
            // also not sure if it's case sensitive when searching the base..
            return db.StudentsRepository.Get(s => s.FirstName.ToLower().StartsWith(start));

            // of course we'd use a studentDto above, with FirstName, LastName, SchoolClass and Id.
        }
    }
}