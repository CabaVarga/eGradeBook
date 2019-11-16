using eGradeBook.Models;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Repositories;
using eGradeBook.Services.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Takings service
    /// </summary>
    public class TakingsService : ITakingsService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public TakingsService(IUnitOfWork db)
        {
            this.db = db;
        }


        /// <summary>
        /// Get taking by Id return dto
        /// </summary>
        /// <param name="takingId"></param>
        /// <returns></returns>
        public TakingDto GetTakingById(int takingId)
        {
            logger.Info("Get taking dto by Id {@takingId}", takingId);

            var taking = db.TakingsRepository.GetByID(takingId);

            return Converters.TakingsConverter.TakingToTakingDto(taking);
        }

        /// <summary>
        /// Create taking from taking dto
        /// </summary>
        /// <param name="takingDto"></param>
        /// <returns></returns>
        public TakingDto CreateTakingDto(TakingDto takingDto)
        {
            logger.Info("Create taking {@takingDto} return dto", takingDto);

            var taking = new Taking()
            {
                EnrollmentId = takingDto.EnrollmentId,
                ProgramId = takingDto.ProgramId
            };

            db.TakingsRepository.Insert(taking);
            db.Save();

            return Converters.TakingsConverter.TakingToTakingDto(taking);
        }

        /// <summary>
        /// Get all takings and return dtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TakingDto> GetAllTakings()
        {
            logger.Info("Get all takings and return dtos");

            var takings = db.TakingsRepository.Get().Select(t => Converters.TakingsConverter.TakingToTakingDto(t));

            return takings;
        }

        public IEnumerable<TakingDto> GetTakingsByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var takings = db.TakingsRepository.Get(
                g => (courseId != null ? g.Program.Teaching.CourseId == courseId : true) &&
                    (teacherId != null ? g.Program.Teaching.TeacherId == teacherId : true) &&
                    (classRoomId != null ? g.Program.ClassRoomId == classRoomId : true) &&
                    (studentId != null ? g.Enrollment.StudentId == studentId : true) &&
                    (parentId != null ? g.Enrollment.Student.StudentParents.Any(sp => sp.ParentId == parentId) : true) &&
                    (schoolGrade != null ? g.Program.ClassRoom.ClassGrade == schoolGrade : true))
                    .Select(g => Converters.TakingsConverter.TakingToTakingDto(g));

            return takings;
        }

        public TakingDto UpdateTaking(int takingId, TakingDto takingDto)
        {
            var taking = db.TakingsRepository.GetByID(takingId);

            if (taking == null)
            {
                return null;
            }

            return null;
        }

        public TakingDto DeleteTakingById(int takingId)
        {
            var taking = db.TakingsRepository.GetByID(takingId);
            if (taking == null)
            {
                return null;
            }
            var deletedTaking = Converters.TakingsConverter.TakingToTakingDto(taking);

            db.TakingsRepository.Delete(taking);
            db.Save();

            return deletedTaking;
        }
    }
}