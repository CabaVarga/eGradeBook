using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Repositories;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with students and related tasks
    /// </summary>
    public class StudentsService : IStudentsService
    {
        private IUnitOfWork db;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public StudentsService(IUnitOfWork db)
        {
            this.db = db;
        }

        /// <summary>
        /// Delete a student from the system
        /// NOTE: This should probably not be here but in accounts....
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public StudentDto DeleteStudent(int studentId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve all students
        /// </summary>
        /// <returns>IEnumerable of StudentUser</returns>
        public IEnumerable<StudentUser> GetAllStudents()
        {
            return db.StudentsRepository.Get();
        }

        /// <summary>
        /// Retrieve all students
        /// </summary>
        /// <returns>IEnumerable of StudentDto</returns>
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

        /// <summary>
        /// Retrieve a student by Id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public StudentDto GetStudentById(int studentId)
        {
            var student = db.StudentsRepository.Get(s => s.Id == studentId).FirstOrDefault();

            if (student == null)
            {
                return null;
            }

            return Converters.StudentsConverter.StudentToStudentDto(student);
        }

        /// <summary>
        /// Retrieve a student by Id, another version
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve students whose name starts with the provided string
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public IEnumerable<StudentUser> GetStudentsByNameStartingWith(string start)
        {
            // as a query i'm not sure it will accept the tolower stuff
            // also not sure if it's case sensitive when searching the base..
            return db.StudentsRepository.Get(s => s.FirstName.ToLower().StartsWith(start));

            // of course we'd use a studentDto above, with FirstName, LastName, SchoolClass and Id.
        }

        /// <summary>
        /// Update student data
        /// NOTE probably not here but in auth repository....
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        public StudentDto UpdateStudent(int studentId, StudentDto student)
        {
            var updatedStudent = db.StudentsRepository.Get(s => s.Id == studentId).FirstOrDefault();

            if (updatedStudent == null)
            {
                return null;
            }

            updatedStudent.FirstName = student.FirstName;
            updatedStudent.LastName = student.LastName;

            db.StudentsRepository.Update(updatedStudent);
            db.Save();

            return Converters.StudentsConverter.StudentToStudentDto(updatedStudent);
        }
    }
}