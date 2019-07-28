using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.Students;
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

        public IEnumerable<StudentUser> GetAllStudents()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<StudentDto> GetAllStudentsDto()
        {
            return db.StudentsRepository.Get()
                .Select(s => new StudentDto()
                {
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    ClassRoom = s.SchoolClass.Name,
                    StudentId = s.Id,
                    ClassRoomId = s.ClassRoomId
                });

        }

        public StudentDto GetStudentByIdDto(int studentId)
        {
            StudentUser student = db.StudentsRepository.Get(s => s.Id == studentId)
//                .OfType<StudentUser>()
                .FirstOrDefault();

            // ZANIMLJIVO
            // GetById ako je podmetnut Id od GradeBookUsera koji nije StudentUser -> pokusava da vrati istog i program puca
            // Sa druge strane, Get sa filterom radi i bez provere OfType<StudentUser> ???

            if (student == null)
            {
                return null;
            }

            return new StudentDto()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                StudentId = student.Id,
                ClassRoom = student.SchoolClass?.Name,
                ClassRoomId = student.SchoolClass?.Id
            };
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