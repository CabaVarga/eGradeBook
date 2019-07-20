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
    }
}