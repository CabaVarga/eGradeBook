using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Repositories;
using eGradeBook.Services.Exceptions;
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
        private Lazy<IClassRoomsService> classRoomsService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public StudentsService(IUnitOfWork db, ILogger logger,
            Lazy<IClassRoomsService> classRoomsService)
        {
            this.db = db;
            this.logger = logger;
            this.classRoomsService = classRoomsService;
        }

        public TakingDto AssignCourseToStudent(StudentCourseDto course)
        {
            logger.Info("Service received request for assigning a course to a student {@course}", course);

            StudentUser student = db.StudentsRepository.Get(s => s.Id == course.StudentId).FirstOrDefault();

            if (student == null)
            {
                return null;
            }

            Course theCourse = db.CoursesRepository.Get(c => c.Id == course.CourseId).FirstOrDefault();

            if (theCourse == null)
            {
                return null;
            }

            Program program = db.ProgramsRepository.Get(p => p.Teaching.Course.Id == course.CourseId).FirstOrDefault();

            if (program == null)
            {
                return null;
            }

            Taking taking = new Taking()
            {
                Program = program,
                Student = student
            };

            db.TakingsRepository.Insert(taking);
            db.Save();

            return Converters.TakingsConverter.TakingToTakingDto(taking);
        }

        /// <summary>
        /// Delete a student from the system
        /// NOTE: This should probably not be here but in accounts....
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<StudentDto> DeleteStudent(int studentId)
        {
            logger.Info("Service received request for deleting a student {studentId}", studentId);

            var deletedStudent = db.StudentsRepository.Get(s => s.Id == studentId).FirstOrDefault();

            if (deletedStudent == null)
            {
                return null;
            }

            var result = await db.AuthRepository.DeleteUser(studentId);

            if (!result.Succeeded)
            {
                logger.Error("Student removal failed {errors}", result.Errors);
                //return null;
                throw new ConflictException("Delete student failed in auth repo");
            }

            return Converters.StudentsConverter.StudentToStudentDto(deletedStudent);
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
        public StudentUser GetStudentById(int studentId)
        {
            logger.Info("Service received request for returning a student by Id {studentId}", studentId);

            var student = db.StudentsRepository.Get(s => s.Id == studentId).FirstOrDefault();

            if (student == null)
            {
                logger.Info("Student {@studentId} not found", studentId);
                var ex = new StudentNotFoundException(string.Format("Student {0} not found", studentId));
                ex.Data.Add("studentId", studentId);
                throw ex;
            }

            return student;
        }

        /// <summary>
        /// Retrieve a student by Id, another version
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public StudentDto GetStudentByIdDto(int studentId)
        {
            logger.Info("Service received request for returning a student by Id {studentId}", studentId);

            StudentUser student = GetStudentById(studentId);

            if (student == null)
            {
                return null;
            }

            return Converters.StudentsConverter.StudentToStudentDto(student);
        }

        public StudentReportDto GetStudentReport(int studentId)
        {
            StudentUser student = GetStudentById(studentId);

            return Converters.StudentsConverter.StudentToStudentReportDto(student);
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

        public IEnumerable<StudentDto> GetStudentsByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var students = db.StudentsRepository.Get(
                g => (courseId != null ? g.Takings.Any(t => t.Program.Teaching.Course.Id == courseId) : true) &&
                    (teacherId != null ? g.Takings.Any(t => t.Program.Teaching.TeacherId == teacherId) : true) &&
                    (classRoomId != null ? g.Takings.Any(t => t.Program.Course.Id == classRoomId) : true) &&
                    (studentId != null ? g.Id == studentId : true) &&
                    (parentId != null ? g.StudentParents.Any(sp => sp.Parent.Id  == parentId) : true) &&
                    (schoolGrade != null ? g.ClassRoom.ClassGrade == schoolGrade : true))
                    .Select(g => Converters.StudentsConverter.StudentToStudentDto(g));

            return students;
        }

        public bool IsParent(int studentId, int parentId)
        {
            StudentParent studentParent = db.StudentParentsRepository.Get(sp => sp.StudentId == studentId && sp.ParentId == parentId).FirstOrDefault();

            if (studentParent == null)
            {
                return false;
            }

            return true;
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