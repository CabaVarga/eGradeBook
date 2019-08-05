using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Repositories;
using NLog;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with students and related tasks
    /// </summary>
    public class StudentsService : IStudentsService
    {
        private IUnitOfWork db;
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public StudentsService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public void AssignCourseToStudent(StudentCourseDto course)
        {
            logger.Info("Service received request for assigning a course to a student {@course}", course);

            StudentUser student = db.StudentsRepository.Get(s => s.Id == course.StudentId).FirstOrDefault();
            Course theCourse = db.CoursesRepository.Get(c => c.Id == course.CourseId).FirstOrDefault();

            Program program = db.ProgramsRepository.Get(p => p.Teaching.Course.Id == course.CourseId).FirstOrDefault();

            Taking taking = new Taking()
            {
                Program = program,
                Student = student
            };

            db.TakingsRepository.Insert(taking);
            db.Save();
        }

        /// <summary>
        /// Delete a student from the system
        /// NOTE: This should probably not be here but in accounts....
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public StudentDto DeleteStudent(int studentId)
        {
            logger.Info("Service received request for deleting a student {studentId}", studentId);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve all students
        /// </summary>
        /// <returns>IEnumerable of StudentUser</returns>
        public IEnumerable<StudentUser> GetAllStudents()
        {
            logger.Info("Service received request for returning all students");
            return db.StudentsRepository.Get();
        }

        /// <summary>
        /// Retrieve all students
        /// </summary>
        /// <returns>IEnumerable of StudentDto</returns>
        public IEnumerable<StudentDto> GetAllStudentsDto()
        {
            logger.Info("Service received request for returning all students");

            return db.StudentsRepository.Get()
                .Select(s => Converters.StudentsConverter.StudentToStudentDto(s));

        }

        /// <summary>
        /// Retrieve all students and their parents
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StudentWithParentsDto> GetAllStudentsWithParents()
        {
            logger.Info("Service received request for returning all students with their parents");
            return db.StudentsRepository.Get()
                .Select(s => Converters.StudentsConverter.StudentToStudentWithParentsDto(s));
        }

        /// <summary>
        /// Retrieve a student by Id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public StudentDto GetStudentById(int studentId)
        {
            logger.Info("Service received request for returning a student by Id {studentId}", studentId);

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
            logger.Info("Service received request for returning a student by Id {studentId}", studentId);

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
                ClassRoom = student.ClassRoom?.Name,
                ClassRoomId = student.ClassRoom?.Id
            };
        }

        /// <summary>
        /// Retrieve students whose first name starts with the provided string
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public IEnumerable<StudentDto> GetStudentsByFirstNameStartingWith(string start)
        {
            logger.Info("Service received request for returning students with First name starting with {start}", start);
            // as a query i'm not sure it will accept the tolower stuff
            // also not sure if it's case sensitive when searching the base..
            return db.StudentsRepository.Get(s => s.FirstName.ToLower().StartsWith(start))
                .Select(s => Converters.StudentsConverter.StudentToStudentDto(s));

            // of course we'd use a studentDto above, with FirstName, LastName, SchoolClass and Id.
        }

        /// <summary>
        /// Retrieve students whose last name starts with the provided string
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public IEnumerable<StudentDto> GetStudentsByLastNameStartingWith(string start)
        {
            logger.Info("Service received request for returning students with Last name starting with {start}", start);

            return db.StudentsRepository.Get(s => s.LastName.ToLower().StartsWith(start))
                .Select(s => Converters.StudentsConverter.StudentToStudentDto(s));
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
            logger.Info("Service received request for updating data for student {studentId} with data {@student}", studentId, student);

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