using eGradeBook.Models;
using eGradeBook.Models.Dtos.FinalGrades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Converter for Final Grades
    /// TODO implement the converter
    /// </summary>
    public static class FinalGradesConverter
    {
        public static FinalGradeDto FinalGradeToFinalGradeDto(FinalGrade finalGrade)
        {
            return new FinalGradeDto()
            {
                FinalGradeId = finalGrade.Id,
                FinalGrade = finalGrade.GradePoint,
                SchoolGrade = finalGrade.Taking.Program.ClassRoom.ClassGrade,
                Semester = finalGrade.SchoolTerm,
                StudentId = finalGrade.Taking.Student.Id,
                CourseId = finalGrade.Taking.Program.Teaching.Course.Id
            };
        }
    }
}