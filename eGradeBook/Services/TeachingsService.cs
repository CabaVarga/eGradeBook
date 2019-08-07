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

        public TeachingsService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
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
            return db.TeachingAssignmentsRepository.Get(ta => ta.CourseId == courseId && ta.TeacherId == teacherId).FirstOrDefault();
        }

        public Teaching DeleteTeaching(TeachingDto teachingDto)
        {
            return DeleteTeaching(teachingDto.CourseId, teachingDto.TeacherId);
        }

        public Teaching DeleteTeaching(int courseId, int teacherId)
        {
            Teaching teaching = GetTeaching(courseId, teacherId);

            db.TeachingAssignmentsRepository.Delete(teaching);
            db.Save();

            return teaching;
        }

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
    }
}