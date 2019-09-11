using eGradeBook.Models;
using eGradeBook.Models.Dtos.ClassRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Conversion of classroom related resources
    /// </summary>
    public static class ClassRoomConverter
    {
        /// <summary>
        /// Convert classroom entity to classroom dto
        /// TODO consolidate, program null or not null and so on
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ClassRoomDto ClassRoomToClassRoomDto(ClassRoom c)
        {
            return new ClassRoomDto()
            {
                ClassRoomId = c.Id,
                Name = c.Name,
                SchoolGrade = c.ClassGrade,
                // use student dto -- except we don't want circular references...
                Students = c.Students?.Select(s => new ClassRoomDto.ClassRoomStudentDto()
                {
                    FullName = s.FirstName + " " + s.LastName,
                    StudentId = s.Id
                }).ToList(),
                // Use program dto -- except we don't want circular references
                Program = c.Program?.Select(p => new ClassRoomDto.ClassRoomProgramDto()
                {
                    Course = p.Course.Name,
                    Teacher = p.Teaching.Teacher.FirstName + " " + p.Teaching.Teacher.LastName,
                    CourseId = p.CourseId,
                    TeacherId = p.Teaching.TeacherId,
                    TeachingHours = p.WeeklyHours
                }).ToList()
            };
        }

        /// <summary>
        /// Convert classroom entity to basic report for that entity
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static ClassRoomBasicReportDto ClassRoomToClassRoomBasicReportDto(ClassRoom c)
        {
            return new ClassRoomBasicReportDto()
            {
                ClassRoomId = c.Id,
                ClassRoomName = c.Name,
                ClassRoomGrade = c.ClassGrade,
                Courses = c.Program?.Select(p => new ClassRoomBasicReportDto.CourseDto()
                {
                    CourseId = p.Teaching.Course.Id,
                    CourseName = p.Teaching.Course.Name
                }),
                Students = c.Students?.Select(s => new ClassRoomBasicReportDto.StudentDto()
                {
                    StudentId = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName
                })
            };
        }

        public static ClassRoomFullReportDto ClassRoomToClassRoomFullReportDto(ClassRoom c)
        {
            return new ClassRoomFullReportDto()
            {
                ClassRoomId = c.Id,
                ClassRoomName = c.Name,
                ClassRoomGrade = c.ClassGrade,
                Courses = c.Program?.Select(p => new ClassRoomFullReportDto.CourseDto()
                {
                    CourseId = p.Teaching.Course.Id,
                    CourseName = p.Teaching.Course.Name
                }),
                Students = c.Students?.Select(s => new ClassRoomFullReportDto.StudentDto()
                {
                    StudentId = s.Id,
                    FirstName = s.FirstName,
                    LastName = s.LastName
                }),
                Grades = c.Students?
                    .SelectMany(s => s.Takings)
                    .SelectMany(t => t.Grades)
                    .Select(g => new ClassRoomFullReportDto.GradeDto()
                {
                    GradeId = g.Id,
                    StudentId = g.Taking.Student.Id,
                    CourseId = g.Taking.Program.Teaching.Course.Id,
                    GradePoint = g.GradePoint,
                    AssignmentDate = g.Assigned,
                    Semester = g.SchoolTerm,
                    Notes = g.Notes
                }),
                FinalGrades = c.Students?
                    .SelectMany(s => s.Takings)
                    .SelectMany(t => t.FinalGrades)
                    .Select(g => new ClassRoomFullReportDto.FinalGradeDto()
                {
                    FinalGradeId = g.Id,
                    StudentId = g.Taking.Student.Id,
                    CourseId = g.Taking.Program.Teaching.Course.Id,
                    FinalGrade = g.GradePoint,
                    Semester = g.SchoolTerm,                        
                }),
            };
        }
    }
}