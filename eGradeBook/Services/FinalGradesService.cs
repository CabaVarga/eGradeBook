using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.FinalGrades;
using eGradeBook.Repositories;
using NLog;

namespace eGradeBook.Services
{
    /// <summary>
    /// The service layer for working with final grades
    /// </summary>
    public class FinalGradesService : IFinalGradesService
    {
        /// <summary>
        /// We will be using the Unit of work as the repository layer orchestrator
        /// </summary>
        private IUnitOfWork db;
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public FinalGradesService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public FinalGradeDto CreateFinalGrade(FinalGradeDto finalGradeDto)
        {
            // no logging for now
            FinalGrade createdGrade = new FinalGrade
            {
                Assigned = finalGradeDto.AssignmentDate,
                GradePoint = finalGradeDto.FinalGradePoint,
                Notes = finalGradeDto.Notes,
                SchoolTerm = finalGradeDto.Semester,
                TakingId = finalGradeDto.TakingId              
            };

            db.FinalGradesRepository.Insert(createdGrade);
            db.Save();

            // for now
            return Converters.FinalGradesConverter.FinalGradeToFinalGradeDto(createdGrade);
        }

        public FinalGradeDto DeleteFinalGrade(int finalGradeId)
        {
            var deletedFinalGrade = db.FinalGradesRepository.GetByID(finalGradeId);

            if (deletedFinalGrade == null)
            {
                return null;
            }

            // try to convert before deletion?
            var deletedFinalGradeDto = Converters.FinalGradesConverter.FinalGradeToFinalGradeDto(deletedFinalGrade);

            db.FinalGradesRepository.Delete(deletedFinalGrade);
            db.Save();

            return deletedFinalGradeDto;
        }

        /// <summary>
        /// Retrieve all final grades and convert them to Dtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FinalGradeDto> GetAllFinalGrades()
        {
            return db.FinalGradesRepository.Get().Select(fg => Converters.FinalGradesConverter.FinalGradeToFinalGradeDto(fg));
        }

 
        public FinalGradeDto GetFinalGradeById(int finalGradeId)
        {
            logger.Info("Get final grade dto by Id {@finalGradeId}", finalGradeId);

            FinalGrade finalGrade = db.FinalGradesRepository.GetByID(finalGradeId);

            if (finalGrade == null)
            {
                return null;
            }

            return Converters.FinalGradesConverter.FinalGradeToFinalGradeDto(finalGrade);
        }

        public IEnumerable<FinalGradeDto> GetFinalGradesByQuery(int? gradeId = null, int? finalGradeId = null, int? courseId = null, int? teacherId = null, int? classRoomId = null, int? studentId = null, int? parentId = null, int? semester = null, int? schoolGrade = null, int? grade = null, int? finalGrade = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return db.FinalGradesRepository.Get(
                filter:
                g => (gradeId != null ? g.Taking.Grades.Any(gr => gr.Id == gradeId) : true) &&
                    (finalGradeId != null ? g.Id == finalGradeId : true) &&
                    (courseId != null ? g.Taking.Program.Teaching.CourseId == courseId : true) &&
                    (teacherId != null ? g.Taking.Program.Teaching.TeacherId == teacherId : true) &&
                    (classRoomId != null ? g.Taking.Program.ClassRoomId == classRoomId : true) &&
                    (studentId != null ? g.Taking.Enrollment.StudentId == studentId : true) &&
                    (parentId != null ? g.Taking.Enrollment.Student.StudentParents.Any(sp => sp.ParentId == parentId) : true) &&
                    (semester != null ? g.SchoolTerm == semester : true) &&
                    (schoolGrade != null ? g.Taking.Program.ClassRoom.ClassGrade == schoolGrade : true) &&
                    (grade != null ? g.Taking.Grades.Any(gr => gr.GradePoint == grade) : true) &&
                    (finalGrade != null ? g.GradePoint == finalGrade : true) &&
                    (fromDate != null ? g.Assigned >= fromDate : true) &&
                    (toDate != null ? g.Assigned <= toDate : true))
                    .Select(g => Converters.FinalGradesConverter.FinalGradeToFinalGradeDto(g));
                    }

        public FinalGradeDto UpdateFinalGrade(int finalGradeId, FinalGradeDto finalGradeDto)
        {
            var finalGradeToUpdate = db.FinalGradesRepository.GetByID(finalGradeId);

            if (finalGradeToUpdate == null)
            {
                return null;
            }

            // update:
            finalGradeToUpdate.Assigned = finalGradeDto.AssignmentDate;
            finalGradeToUpdate.GradePoint = finalGradeDto.FinalGradePoint;
            finalGradeToUpdate.Notes = finalGradeDto.Notes;

            db.FinalGradesRepository.Update(finalGradeToUpdate);
            db.Save();

            return Converters.FinalGradesConverter.FinalGradeToFinalGradeDto(finalGradeToUpdate);
        }
    }
}