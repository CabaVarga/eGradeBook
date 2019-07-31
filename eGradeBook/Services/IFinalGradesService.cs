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
        IEnumerable<FinalGradeDto> GetAllFinalGradesDto();

        /// <summary>
        /// Get all final grades for the given student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<FinalGradeDto> GetAllFinalGradesForStudent(int studentId);

        /// <summary>
        /// Get all final grades for the given course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        IEnumerable<FinalGradeDto> GetAllFinalGradesForCourse(int courseId);
    }
}