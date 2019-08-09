using eGradeBook.Models;
using eGradeBook.Models.Dtos.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    public static class ProgramsConverter
    {
        public static ProgramDto ProgramToProgramDto(Program program)
        {
            return new ProgramDto()
            {
                ProgramId = program.Id,
                CourseId = program.CourseId,
                TeacherId = program.Teaching.TeacherId,
                TeachingId = program.Teaching.Id,
                ClassRoomId = program.ClassRoomId,
                WeeklyHours = program.WeeklyHours,
                CourseName= program.Teaching.Course.Name,
                TeacherName = program.Teaching.Teacher.UserName,
                ClassRoomName = program.ClassRoom.Name,
                SchoolGrade = program.ClassRoom.ClassGrade
            };
        }
    }
}