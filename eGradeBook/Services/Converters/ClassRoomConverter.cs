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
    }
}