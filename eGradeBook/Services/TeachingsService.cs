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
using System.Security.Claims;
using System.Threading;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Teachings Service
    /// </summary>
    public class TeachingsService : ITeachingsService
    {
        private IUnitOfWork db;
        private ILogger logger;
        private Lazy<ITeachersService> teachersService;
        private Lazy<ICoursesService> coursesService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        /// <param name="teachersService"></param>
        /// <param name="coursesService"></param>
        public TeachingsService(IUnitOfWork db, ILogger logger, 
            Lazy<ITeachersService> teachersService,
            Lazy<ICoursesService> coursesService)
        {
            this.db = db;
            this.logger = logger;
            this.teachersService = teachersService;
            this.coursesService = coursesService;
        }

        /// <summary>
        /// Create teaching from teachingDto
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        public Teaching CreateTeaching(TeachingDto teachingDto)
        {
            return CreateTeaching(teachingDto.CourseId, teachingDto.TeacherId);
        }

        /// <summary>
        /// Create teaching from courseId and teacherId
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public Teaching CreateTeaching(int courseId, int teacherId)
        {
            Course course = coursesService.Value.GetCourseById(courseId);

            TeacherUser teacher = teachersService.Value.GetTeacherById(teacherId);
            
            Teaching teaching = new Teaching()
            {
                Course = course,
                Teacher = teacher
            };

            db.TeachingAssignmentsRepository.Insert(teaching);
            db.Save();

            return teaching;
        }

        /// <summary>
        /// Create teaching from teachingDto and return a teachingDto
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        public TeachingDto CreateTeachingDto(TeachingDto teachingDto)
        {
            Teaching teaching = CreateTeaching(teachingDto);

            return Converters.TeachingsConverter.TeachingToTeachingDto(teaching);
        }

        /// <summary>
        /// Get all teachings
        /// NOTE not very helpful
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Teaching> GetAllTeachings()
        {
            return db.TeachingAssignmentsRepository.Get();
        }

        /// <summary>
        /// Get teaching by Id -- throws if not found
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        public Teaching GetTeachingById(int teachingId)
        {
            logger.Info("Get teaching by Id {@teachingId}", teachingId);

            var teaching = db.TeachingAssignmentsRepository.Get(ta => ta.Id == teachingId).FirstOrDefault();

            if (teaching == null)
            {
                // TODO find a way out of this madness, cant be teacher and course in one place and teaching in another place...
                logger.Info("Teaching not found for Id {@teachingId}", teachingId);
                var ex = new TeachingNotFoundException(string.Format("Teaching not found for Id {0}", teachingId));
                ex.Data.Add("teachingId", teachingId);
                throw ex;
            }

            return teaching;
        }

        /// <summary>
        /// Get teaching by Id and return a teachingDto
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        public TeachingDto GetTeachingDtoById(int teachingId)
        {
            logger.Info("Get teaching dto by Id {@teachingId}", teachingId);
            Teaching teaching = GetTeachingById(teachingId);

            return Converters.TeachingsConverter.TeachingToTeachingDto(teaching);
        }

        /// <summary>
        /// Get teaching by course and teacher -- throws
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
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
        /// Remove teacher from course
        /// NOTE better use DELETE to courses/{courseId}/teachers/{teachersId}
        /// it is more RESTful
        /// NOTE this is the old method
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
        /// Get all teachings for a given course
        /// NOTE implement with check and returning teachingDtos
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IEnumerable<Teaching> GetAllTeachingsForCourse(int courseId)
        {
            Course course = coursesService.Value.GetCourseById(courseId);

            return db.TeachingAssignmentsRepository.Get(ta => ta.CourseId == courseId);
        }

        /// <summary>
        /// Get all teachings for given teacher
        /// NOTE implement with teacherDtos ?
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public IEnumerable<Teaching> GetAllTeachingsForTeacher(int teacherId)
        {
            TeacherUser teacher = teachersService.Value.GetTeacherById(teacherId);

            return db.TeachingAssignmentsRepository.Get(ta => ta.TeacherId == teacherId);
        }

        /// <summary>
        /// Get all teachings as dtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeachingDto> GetAllTeachingsDtos()
        {
            logger.Info("Get all teachings as dtos");
            return db.TeachingAssignmentsRepository.Get()
                .Select(ta => Converters.TeachingsConverter.TeachingToTeachingDto(ta));
        }

        /// <summary>
        /// Delete a Teaching
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        public TeachingDto DeleteTeaching(int teachingId)
        {
            logger.Info("Attempting to delete Teaching");

            Teaching deletedTeaching = db.TeachingAssignmentsRepository.GetByID(teachingId);

            if (deletedTeaching == null)
            {
                return null;
            }

            var deletedTeachingDto = Converters.TeachingsConverter.TeachingToTeachingDto(deletedTeaching);

            db.TeachingAssignmentsRepository.Delete(teachingId);
            db.Save();


            return deletedTeachingDto;
        }

        /// <summary>
        /// Return teachings for courseId and/or teacherId
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public IEnumerable<TeachingDto> GetTeachingsByParameters(int? courseId, int? teacherId)
        {
            return db.TeachingAssignmentsRepository.Get(filter:
                g => (courseId != null ? g.Course.Id == courseId : true) &&
                    (teacherId != null ? g.Teacher.Id == teacherId : true))
                    .Select(g => Converters.TeachingsConverter.TeachingToTeachingDto(g));
        }
    }
}