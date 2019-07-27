using eGradeBook.Models.Dtos.FinalGrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface IFinalGradesService
    {
        IEnumerable<FinalGradeDto> GetAllFinalGradesDto();
        IEnumerable<FinalGradeDto> GetAllFinalGradesForStudent(int studentId);
        IEnumerable<FinalGradeDto> GetAllFinalGradesForCourse(int courseId);
    }
}