using eGradeBook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Interface for grades service
    /// </summary>
    public interface IGradesService
    {
        /// <summary>
        /// Get all grades
        /// </summary>
        /// <returns></returns>
        IEnumerable<GradeDto> GetAllGrades();

        /// <summary>
        /// Get all grades for the given teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        IEnumerable<GradeDto> GetAllGradesForTeacher(int teacherId);

        /// <summary>
        /// Get all grades for the given student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IEnumerable<GradeDto> GetAllGradesForStudent(int studentId);

        /// <summary>
        /// Get all grades for the given parent
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IEnumerable<GradeDto> GetAllGradesForParent(int parentId);


        /// <summary>
        /// Create (assign) a grade
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="studentId"></param>
        /// <param name="subjectId"></param>
        /// <param name="gradePoint"></param>
        /// <param name="notes"></param>
        /// <returns></returns>
        GradeDto CreateGrade(int teacherId, int studentId, int subjectId, int gradePoint, string notes = null);

        // --- PRETRAZIVANJE --- Najvise se spominje povodom ocene
        // po studentu, razredu, nastavniku, predmetu, roditelju, polugodistu, odeljenju
        // mozda dodati jedno mega-pretrazivanje sa navedenim URI parametrima?
        // da probam....

        /// <summary>
        /// Get grades by multiple parameters.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="gradeId"></param>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        /// <param name="semesterId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        IEnumerable<GradeDto> GetGradesByParameters(int? studentId, int? gradeId, int? teacherId, int? courseId, int? semesterId, int? classId);

        /// <summary>
        /// Get grades grouped by courses
        /// </summary>
        /// <returns></returns>
        IEnumerable<GradeDto> GetGradesByCourses();
    }
}