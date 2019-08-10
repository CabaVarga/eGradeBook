using eGradeBook.Models;
using eGradeBook.Models.Dtos.Takings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    public static class TakingsConverter
    {
        public static TakingDto TakingToTakingDto(Taking taking)
        {
            return new TakingDto()
            {
                TakingId = taking.Id,
                StudentId = taking.Student.Id,
                CourseId = taking.Program.TeachingPrograms.FirstOrDefault().Teaching.Course.Id,
                TeacherId = taking.Program.TeachingPrograms.FirstOrDefault().Teaching.Teacher.Id,
                TeachingId = taking.Program.TeachingPrograms.FirstOrDefault().Teaching.Id,
                ClassRoomId = taking.Program.ClassRoom.Id,
                ProgramId = taking.Program.Id,
                WeeklyHours = taking.Program.WeeklyHours,
                CourseName = taking.Program.TeachingPrograms.FirstOrDefault().Teaching.Course.Name,
                TeacherName = taking.Program.TeachingPrograms.FirstOrDefault().Teaching.Teacher.UserName,
                StudentName = taking.Student.UserName,
                ClassRoomName = taking.Program.ClassRoom.Name,
                SchoolGrade = taking.Program.ClassRoom.ClassGrade
            };
        }
    }
}