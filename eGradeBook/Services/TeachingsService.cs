using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Teachings;
using eGradeBook.Repositories;
using eGradeBook.Services.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public class TeachingsService : ITeachingsService
    {
        private IUnitOfWork db;
        private ILogger logger;
        private Lazy<ITeachersService> teachersService;
        private Lazy<ICoursesService> coursesService;

        public TeachingsService(IUnitOfWork db, ILogger logger, 
            Lazy<ITeachersService> teachersService,
            Lazy<ICoursesService> coursesService)
        {
            this.db = db;
            this.logger = logger;
            this.teachersService = teachersService;
            this.coursesService = coursesService;
        }

        public Teaching CreateTeaching(TeachingDto teachingDto)
        {
            return CreateTeaching(teachingDto.CourseId, teachingDto.TeacherId);
        }

        public Teaching CreateTeaching(int courseId, int teacherId)
        {
            TeacherUser teacher = db.TeachersRepository.Get(t => t.Id == teacherId).FirstOrDefault();
            Course course = db.CoursesRepository.GetByID(courseId);

            Teaching teaching = new Teaching()
            {
                Course = course,
                Teacher = teacher
            };

            db.TeachingAssignmentsRepository.Insert(teaching);
            db.Save();

            return teaching;
        }

        public Teaching GetTeaching(TeachingDto teachingDto)
        {
            return GetTeaching(teachingDto.CourseId, teachingDto.TeacherId);
        }


        public Teaching GetTeaching(int courseId, int teacherId)
        {
            Course course = coursesService.Value.GetCourseById(courseId);

            TeacherUser teacherUser = teachersService.Value.GetTeacherById(teacherId);

            var teaching = db.TeachingAssignmentsRepository.Get(ta => ta.CourseId == courseId && ta.TeacherId == teacherId).FirstOrDefault();

            if (teaching == null)
            {
                logger.Info("Teaching not found for course {@courseId} and teacher {@teacherId}", courseId, teacherId);
                var ex = new TeachingNotFoundException(string.Format("Teaching not found for course {0} and teacher {1}", courseId, teacherId));
                ex.Data.Add("teacherId", teacherId);
                ex.Data.Add("courseId", courseId);
                throw ex;
            }

            return teaching;
        }

        /// <summary>
        /// Delete teaching
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        public Teaching DeleteTeaching(TeachingDto teachingDto)
        {
            return DeleteTeaching(teachingDto.CourseId, teachingDto.TeacherId);
        }

        /// <summary>
        /// Delete teaching
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public Teaching DeleteTeaching(int courseId, int teacherId)
        {
            Teaching teaching = GetTeaching(courseId, teacherId);

            db.TeachingAssignmentsRepository.Delete(teaching);
            db.Save();

            return teaching;
        }

        /// <summary>
        /// Assign teacher to course
        /// NOTE another, better way (if) is using the POST to create teaching...
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public Teaching AssignTeacherToCourse(int courseId, int teacherId)
        {
            var course = db.CoursesRepository.GetByID(courseId);

            if (course == null) return null;

            var teacher = db.TeachersRepository.GetByID(teacherId);

            if (teacher == null) return null;

            try
            {
                var teaching = new Teaching()
                {
                    Course = course,
                    Teacher = teacher
                };

                db.TeachingAssignmentsRepository.Insert(teaching);
                db.Save();

                return teaching;
            }

            catch (EntityException ex)
            {
                // entity framework base exception
                return null;
            }

            catch (DbException ex)
            {
                // generic abstract data source exception
                return null;
            }
        }

        /// <summary>
        /// This is a reporting tool
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeachingsByCoursesDto> GetAllTeachingAssignmentsByCourses()
        {
            logger.Info("Retrieving all teachings by course");
            var teachings = db.TeachingAssignmentsRepository.Get()
                .GroupBy(ta => ta.Course)
                .Select(ta => new TeachingsByCoursesDto()
                {
                    Course = ta.Key.ColloqialName,
                    Teachers = ta.Select(c => c.Teacher.UserName).OrderBy(t => t).ToList()
                });

            return teachings;
        }

        /// <summary>
        /// Also a reporting tool
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeachingsByTeachersDto> GetAllTeachingAssignmentsByTeachers()
        {
            var mlcs = NLog.MappedDiagnosticsContext.GetNames().ToList();
            var names = NLog.GlobalDiagnosticsContext.GetNames().ToList();
            var mdlcs = NLog.MappedDiagnosticsLogicalContext.GetNames().ToList();

            logger.Info("Retrieving all teachings by teachers");

            var teachings = db.TeachingAssignmentsRepository.Get()
                .GroupBy(ta => ta.Teacher)
                .Select(ta => new TeachingsByTeachersDto()
                {
                    Teacher = ta.Key.UserName,
                    TeacherId = ta.Key.Id,
                    Courses = ta.Select(c => (object) (new { Name = c.Course.Name, CourseId = c.Course.Id })).ToList()
                });

            return teachings;
        }

        /// <summary>
        /// Remove teacher from course
        /// NOTE better use DELETE to courses/{courseId}/teachers/{teachersId}
        /// it is more RESTful
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public Teaching RemoveTeacherFromCourse(int courseId, int teacherId)
        {
            // 4 errors:

            // 1) courseId not found
            // 2) teacherId not found
            // 3) teaching not found
            // 4) cannot remove association because it is in use


            Course course = db.CoursesRepository.GetByID(courseId);

            if (course == null)
            {
                logger.Error("Course {@courseId} not found", courseId);
                var ex = new CourseNotFoundException(String.Format("Course {0} not found", courseId));
                ex.Data.Add("courseId", courseId);
                throw ex;
            }

            TeacherUser teacher = db.TeachersRepository.Get(t => t.Id == teacherId).FirstOrDefault();

            if (teacher == null)
            {
                logger.Error("Teacher {@teacherId} not found", teacherId);
                var ex = new TeacherNotFoundException(String.Format("Teacher {0} not found", courseId));
                ex.Data.Add("teacherId", teacherId);
                throw ex;
            }

            // Maybe we don't need to check Course and Teacher at all?
            Teaching teaching = db.TeachingAssignmentsRepository.Get(ta => ta.CourseId == courseId && ta.TeacherId == teacherId).FirstOrDefault();

            if (teaching == null)
            {
                logger.Error("Teaching assignment for teacher {@teacherId} and course {@courseId} not found", teacherId, courseId);
                var ex = new TeachingNotFoundException(String.Format("Teaching assignment for teacher {0} and course {1} not found", teacherId, courseId));
                ex.Data.Add("teacherId", teacherId);
                ex.Data.Add("courseId", courseId);
                throw ex;
            }

            try
            {
                // Probably another method in service?
                db.TeachingAssignmentsRepository.Delete(teaching);
                db.Save();
            }

            catch (Exception ex)
            {
                logger.Error(ex, "Removal of teaching assignment failed for teacher {@teacherId} and course {@courseId}", teacherId, courseId);
                throw;
            }

            return null;
        }

        public IEnumerable<Teaching> GetAllTeachings()
        {
            return db.TeachingAssignmentsRepository.Get();
        }

        public IEnumerable<Teaching> GetAllTeachingsForCourse(int courseId)
        {
            Course course = coursesService.Value.GetCourseById(courseId);

            return db.TeachingAssignmentsRepository.Get(ta => ta.CourseId == courseId);
        }

        public IEnumerable<Teaching> GetAllTeachingsForTeacher(int teacherId)
        {
            TeacherUser teacher = teachersService.Value.GetTeacherById(teacherId);

            return db.TeachingAssignmentsRepository.Get(ta => ta.TeacherId == teacherId);
        }
    }
}