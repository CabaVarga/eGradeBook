using eGradeBook.Models;
using eGradeBook.Models.Dtos.FinalGrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Interface for Final Grades service
    /// </summary>
    public interface IFinalGradesService
    {
        /// <summary>
        /// Get all final grades
        /// </summary>
        /// <returns></returns>
        IEnumerable<FinalGradeDto> GetAllFinalGrades();




        FinalGradeDto GetFinalGradeById(int finalGradeId);

        FinalGradeDto CreateFinalGrade(FinalGradeDto finalGradeDto);
        FinalGradeDto UpdateFinalGrade(int finalGradeId, FinalGradeDto finalGradeDto);
        FinalGradeDto DeleteFinalGrade(int finalGradeId);

        IEnumerable<FinalGradeDto> GetFinalGradesByQuery(
            int? gradeId = null,
            int? finalGradeId = null,
            int? courseId = null,
            int? teacherId = null,
            int? classRoomId = null,
            int? studentId = null,
            int? parentId = null,
            int? semester = null,
            int? schoolGrade = null,
            int? grade = null,
            int? finalGrade = null,
            DateTime? fromDate = null,
            DateTime? toDate = null);
    }
}