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
                GradeId = grade.Id,
                CourseId = grade.Taking.Program.Teaching.Course.Id,
                TeacherId = grade.Taking.Program.Teaching.TeacherId,
                TeachingId = grade.Taking.Program.Teaching.Id,
                ClassRoomId = grade.Taking.Program.ClassRoomId,
                ProgramId = grade.Taking.Program.Id,
                StudentId = grade.Taking.StudentId,
                TakingId = grade.Taking.Id,
                GradePoint = grade.GradePoint,
                AssignmentDate = grade.Assigned,
                Semester = grade.SchoolTerm,
                Notes = grade.Notes,
                SchoolGrade = grade.Taking.Program.ClassRoom.ClassGrade,
                CourseName = grade.Taking.Program.Course.Name,
                TeacherName = grade.Taking.Program.Teaching.Teacher.UserName,
                ClassRoomName = grade.Taking.Program.ClassRoom.Name,
                StudentName = grade.Taking.Student.UserName,
                WeeklyHours = grade.Taking.Program.WeeklyHours                
            };
        }

        public static GradeNotificationDto GradeToGradeNotificationDto(Grade grade, ParentUser parent)
        {
            return new GradeNotificationDto()
            {
                ParentFirstName = parent.FirstName,
                ParentLastName = parent.LastName,
                ParentEmail = parent.Email,
                StudentFirstName = grade.Taking.Student.FirstName,
                StudentLastName = grade.Taking.Student.LastName,
                TeacherFirstName = grade.Taking.Program.Teaching.Teacher.FirstName,
                TeacherLastName = grade.Taking.Program.Teaching.Teacher.LastName,
                Course = grade.Taking.Program.Teaching.Course.Name,
                ClassRoom = grade.Taking.Program.ClassRoom.Name,
                GradePoint = grade.GradePoint.ToString(),
                Assigned = grade.Assigned.ToShortDateString()
            };
        }
    }
}