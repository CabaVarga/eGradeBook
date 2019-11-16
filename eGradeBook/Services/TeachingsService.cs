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
        public TeachingsService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        /// <summary>
        /// Create teaching from teachingDto and return a teachingDto
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        public TeachingDto CreateTeaching(TeachingDto teachingDto)
        {
            Teaching teaching = new Teaching()
            {
                CourseId = teachingDto.CourseId,
                TeacherId = teachingDto.TeacherId
            };

            db.TeachingAssignmentsRepository.Insert(teaching);
            db.Save();

            return Converters.TeachingsConverter.TeachingToTeachingDto(teaching);
        }




        /// <summary>
        /// Get teaching by Id and return a teachingDto
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        public TeachingDto GetTeachingById(int teachingId)
        {
            logger.Info("Get teaching dto by Id {@teachingId}", teachingId);
            Teaching teaching = db.TeachingAssignmentsRepository.GetByID(teachingId);

            return Converters.TeachingsConverter.TeachingToTeachingDto(teaching);
        }

        /// <summary>
        /// Get all teachings as dtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeachingDto> GetAllTeachings()
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


        public IEnumerable<TeachingDto> GetTeachingsByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var teachings = db.TeachingAssignmentsRepository.Get(
                g => (courseId != null ? g.CourseId == courseId : true) &&
                    (teacherId != null ? g.TeacherId == teacherId : true) &&
                    (classRoomId != null ? g.Programs.Any(p => p.ClassRoomId == classRoomId) : true) &&
                    (studentId != null ? g.Programs.Any(p => p.Takings.Any(t => t.Enrollment.StudentId == studentId)) : true) &&
                    (parentId != null ? g.Programs.Any(p => p.Takings.Any(t => t.Enrollment.Student.StudentParents.Any(sp => sp.ParentId == parentId))) : true) &&
                    (schoolGrade != null ? g.Programs.Any(p => p.ClassRoom.ClassGrade == schoolGrade) : true))
                    .Select(g => Converters.TeachingsConverter.TeachingToTeachingDto(g));

            return teachings;
        }
    }
}