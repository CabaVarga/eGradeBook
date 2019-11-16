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

        GradeDto CreateGrade(GradeDto gradeDto);

        /// <summary>
        /// Get all grades
        /// </summary>
        /// <returns></returns>
        IEnumerable<GradeDto> GetAllGrades();


        // --- PRETRAZIVANJE --- Najvise se spominje povodom ocene
        // po studentu, razredu, nastavniku, predmetu, roditelju, polugodistu, odeljenju
        // mozda dodati jedno mega-pretrazivanje sa navedenim URI parametrima?
        // da probam....

        /// <summary>
        /// Get grades by multiple parameters.
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="parentId"></param>
        /// <param name="gradeId"></param>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        /// <param name="semester"></param>
        /// <param name="schoolGrade"></param>
        /// <param name="grade"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        IEnumerable<GradeDto> GetGradesByParameters(
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
            DateTime? toDate = null);

        IEnumerable<GradeDto> GetGradesByParameters(GradeQueryDto query);


        GradeDto GetGradeById(int gradeId);

        GradeDto UpdateGrade(GradeDto gradeDto);
    }
}