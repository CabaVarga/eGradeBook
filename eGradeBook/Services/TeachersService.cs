using eGradeBook.Models;
using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with teacher entities and related tasks
    /// </summary>
    public class TeachersService : ITeachersService
    {
        private IUnitOfWork db;
        private ITeachingsService teachingsService;
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="teachingsService"></param>
        public TeachersService(IUnitOfWork db, ITeachingsService teachingsService, ILogger logger)
        {
            this.db = db;
            this.teachingsService = teachingsService;
            this.logger = logger;
        }

        /// <summary>
        /// Assign a course to a teacher
        /// </summary>
        /// <param name="assignment"></param>
        public void AssignCourseToTeacher(TeachingAssignmentDto assignment)
        {
            logger.Info("Service received request for assigning a course to a teacher {@teachingAssignment}", assignment);

            teachingsService.AssignTeacherToCourse(assignment.SubjectId, assignment.TeacherId);
        }

        /// <summary>
        /// Delete a teacher user from the system
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public TeacherDto DeleteTeacher(int teacherId)
        {
            logger.Info("Service received request for deleting a teacher {teacherId}", teacherId);

            TeacherUser deletedTeacher = db.TeachersRepository.Get(t => t.Id == teacherId).FirstOrDefault();

            if (deletedTeacher == null)
            {
                return null;
            }

            db.TeachersRepository.Delete(teacherId);
            db.Save();

            return TeachersConverter.TeacherToTeacherDto(deletedTeacher);
        }

        /// <summary>
        /// Retrieve all teachers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeacherDto> GetAllTeachers()
        {
            logger.Info("Service received request for retrieving all teachers");

            return db.TeachersRepository.Get()
                // maybe won't work?
                // also, without include it will take a number of roundtrips to the database...
                .Select(t => TeachersConverter.TeacherToTeacherDto(t));
        }

        /// <summary>
        /// Get all teachers as IEnumerable of TeacherDto
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeacherDto> GetAllTeachersDtos()
        {
            logger.Info("Service received request for retrieving all teachers");

            return db.TeachersRepository.Get()
                .OfType<TeacherUser>()
                .Select(t => new TeacherDto()
                {
                    Name = t.FirstName + " " + t.LastName,
                    TeacherId = t.Id,
                    Courses = t.Teachings.Select(tc => new TeacherDto.CourseList() { Id = tc.SubjectId, Name = tc.Course.Name }).ToList()
//                    Courses = t.Teachings.Select(tc => tc.Course.Name).ToList()
                });
        }

        /// <summary>
        /// Retrieve a teacher by Id
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public TeacherDto GetTeacherById(int teacherId)
        {
            logger.Info("Service received request for retrieving teacher by Id {teacherId}", teacherId);

            var teacher = db.TeachersRepository.Get(t => t.Id == teacherId).FirstOrDefault();
            if (teacher == null)
            {
                return null;
            }

            return TeachersConverter.TeacherToTeacherDto(teacher);
        }

        /// <summary>
        /// Almost same as GetTeacherById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TeacherDto GetTeacherByIdDto(int id)
        {
            logger.Info("Service received request for retrieving teacher by Id {teacherId}", id);
            //            return db.TeachersRepository.Get(t => t.Id == id)
            //                .OfType<TeacherUser>()
            //                .Select(t => new TeacherDto()
            //                {
            //                    Name = t.FirstName + " " + t.LastName,
            //                    TeacherId = t.Id,
            //                    Courses = t.Teachings.Select(tc => new TeacherDto.CourseList() { Id = tc.SubjectId, Name = tc.Course.Name }).ToList()
            ////                    Courses = t.Teachings.Select(tc => tc.Course.Name).ToList()
            //                })
            //                .FirstOrDefault();

            var t = db.TeachersRepository.GetByID(id);


            var dto = new TeacherDto()
            {
                Name = t.FirstName + " " + t.LastName,
                TeacherId = t.Id,
                Courses = t.Teachings.Select(tc => new TeacherDto.CourseList() { Id = tc.SubjectId, Name = tc.Course.Name }).ToList()
                //                    Courses = t.Teachings.Select(tc => tc.Course.Name).ToList()
            };

            return dto;

        }

        /// <summary>
        /// Update teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="teacher"></param>
        /// <returns></returns>
        public TeacherDto UpdateTeacher(int teacherId, TeacherDto teacher)
        {
            logger.Info("Service received request for updating teacher {teacherId} with data {@teacher}", teacherId, teacher);

            throw new NotImplementedException();
        }
    }
}