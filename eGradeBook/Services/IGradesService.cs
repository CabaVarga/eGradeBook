using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Grades;
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
        /// Create (assign) a grade
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        Grade CreateGrade(
            int courseId, int teacherId, int classRoomId, int studentId,
            int schoolTerm, DateTime assigned,
            int gradePoint, string notes = null);

        Grade CreateGrade(GradeDto gradeDto);

        GradeDto CreateGradeDto(GradeDto gradeDto);

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
        // TODO Grade CreateGrade(GradeDto gradeDto);

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

        Grade GetGradeById(int gradeId);
        GradeDto GetGradeDtoById(int gradeId);
    }
}