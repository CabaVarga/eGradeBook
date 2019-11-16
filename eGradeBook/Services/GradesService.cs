using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Grades;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
using eGradeBook.Services.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for everything grades related
    /// </summary>
    public class GradesService : IGradesService
    {
        private IUnitOfWork db;
        private ILogger logger;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        public GradesService(IUnitOfWork db, 
            ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }


        /// <summary>
        /// Create grade from GradeDto return dto
        /// </summary>
        /// <param name="gradeDto"></param>
        /// <returns></returns>
        public GradeDto CreateGrade(GradeDto gradeDto)
        {
            logger.Info("Create grade {@gradeData} return dto", gradeDto);

            var grade = new Grade()
            {
                TakingId = gradeDto.TakingId,
                Assigned = gradeDto.AssignmentDate,
                GradePoint = gradeDto.GradePoint,
                SchoolTerm = gradeDto.Semester,
                Notes = gradeDto.Notes
            };

            db.GradesRepository.Insert(grade);
            db.Save();

            return Converters.GradesConverter.GradeToGradeDto(grade);
        }

        /// <summary>
        /// Retrieve all grades in the system
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GradeDto> GetAllGrades()
        {
            // This is the most basic implementation, with every grade included, no ordering, no grouping..
            var grades = db.GradesRepository.Get()
                .Select(g => GradesConverter.GradeToGradeDto(g));

            return grades;
        }




        /// <summary>
        /// Retrieve grades by multiple parameters.
        /// Specialized methods can call this method.
        /// If the output format needs to be changed, we can supply a converter delegate, maybe?
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="gradeId"></param>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        /// <param name="semesterId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public IEnumerable<GradeDto> GetGradesByParameters(
            int? gradeId = null,
            int? courseId = null, 
            int? teacherId = null, 
            int? classRoomId = null, 
            int? studentId = null,
            int? parentId = null,
            int? semester = null, 
            int? schoolGrade = null,
            int? grade = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            
            //Func<Grade, bool> filter =
            //    g => studentId != null ? g.Taking.StudentId == studentId : true &&
            //        gradeId != null ? g.Taking.Program.ClassRoom.ClassGrade == gradeId : true &&
            //        teacherId != null ? g.Taking.Program.Teaching.TeacherId == teacherId : true &&
            //        courseId != null ? g.Taking.Program.CourseId == courseId : true &&
            //        semesterId != null ? g.SchoolTerm == semesterId : true &&
            //        classId != null ? g.Taking.Program.ClassRoomId == classId : true;

            return db.GradesRepository.Get(
                filter:
                g => (gradeId != null ? g.Id == gradeId : true) &&
                    (courseId != null ? g.Taking.Program.Teaching.CourseId == courseId : true) &&
                    (teacherId != null ? g.Taking.Program.Teaching.TeacherId == teacherId : true) &&
                    (classRoomId != null ? g.Taking.Program.ClassRoomId == classRoomId : true) &&
                    (studentId != null ? g.Taking.Enrollment.StudentId == studentId : true) &&
                    (parentId != null ? g.Taking.Enrollment.Student.StudentParents.Any(sp => sp.ParentId == parentId) : true) &&
                    (semester != null ? g.SchoolTerm == semester : true) &&
                    (schoolGrade != null ? g.Taking.Program.ClassRoom.ClassGrade == schoolGrade : true) &&
                    (grade != null ? g.GradePoint == grade : true) &&
                    (fromDate != null ? g.Assigned >= fromDate : true) &&
                    (toDate != null ? g.Assigned <= toDate : true))
                    .Select(g => GradesConverter.GradeToGradeDto(g));

        }


        /// <summary>
        /// Get grade by Id as dto
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public GradeDto GetGradeById(int gradeId)
        {
            logger.Info("Get grade dto by Id {@gradeId}", gradeId);

            Grade grade = db.GradesRepository.GetByID(gradeId);

            return Converters.GradesConverter.GradeToGradeDto(grade);
        }

        /// <summary>
        /// Get grades by parameters provided in the queryDto
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<GradeDto> GetGradesByParameters(GradeQueryDto query)
        {
            return GetGradesByParameters(
                query.GradeId,
                query.CourseId, query.TeacherId, query.ClassRoomId, query.StudentId, 
                query.ParentId, query.Semester, query.SchoolGrade, query.Grade, query.FromDate, query.ToDate);
        }

        public GradeDto UpdateGrade(GradeDto gradeDto)
        {
            Grade grade = db.GradesRepository.GetByID(gradeDto.GradeId);

            if (grade == null)
            {
                return null;
            }

            grade.Assigned = gradeDto.AssignmentDate;
            grade.GradePoint = gradeDto.GradePoint;
            grade.Notes = gradeDto.Notes;
            grade.SchoolTerm = gradeDto.Semester;

            db.GradesRepository.Update(grade);
            db.Save();

            return GradesConverter.GradeToGradeDto(grade);
        }
    }
}