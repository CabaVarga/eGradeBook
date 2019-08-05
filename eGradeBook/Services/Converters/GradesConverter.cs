using eGradeBook.Models;
using eGradeBook.Models.Dtos.Grades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Converting grades
    /// TODO implement the converter
    /// </summary>
    public static class GradesConverter
    {
        /// <summary>
        /// Convert a grade to Grade Dto
        /// </summary>
        /// <param name="grade"></param>
        /// <returns></returns>
        public static GradeDto GradeToGradeDto(Grade grade)
        {
            return new GradeDto()
            {
                StudentName = grade.Taking.Student.FirstName + " " + grade.Taking.Student.LastName,
                Course = grade.Taking.Program.Course.Name,
                GradePoint = grade.GradePoint,
                TeacherName = grade.Taking.Program.Teaching.Teacher.FirstName + " " + grade.Taking.Program.Teaching.Teacher.LastName,
                CourseId = grade.Taking.Program.CourseId,
                ClassRoomId = grade.Taking.Program.ClassRoomId,
                StudentId = grade.Taking.StudentId,
                TeacherId = grade.Taking.Program.Teaching.TeacherId
                
            };
        }
    }
}