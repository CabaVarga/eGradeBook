using eGradeBook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface IGradesService
    {
        IEnumerable<GradeDto> GetAllGrades();
        IEnumerable<GradeDto> GetAllGradesForTeacher(int teacherId);
        IEnumerable<GradeDto> GetAllGradesForStudent(int studentId);
        IEnumerable<GradeDto> GetAllGradesForParent(int parentId);

        GradeDto CreateGrade(int teacherId, int studentId, int subjectId, int gradePoint, string notes = null);

        // --- PRETRAZIVANJE --- Najvise se spominje povodom ocene
        // po studentu, razredu, nastavniku, predmetu, roditelju, polugodistu, odeljenju
        // mozda dodati jedno mega-pretrazivanje sa navedenim URI parametrima?
        // da probam....

        IEnumerable<GradeDto> GetGradesByParameters(int? studentId, int? gradeId, int? teacherId, int? courseId, int? semesterId, int? classId);
        IEnumerable<GradeDto> GetGradesByCourses();
    }
}