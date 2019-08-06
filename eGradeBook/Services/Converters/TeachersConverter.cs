using eGradeBook.Models;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Conversion for teahcer users 
    /// </summary>
    public static class TeachersConverter
    {
        /// <summary>
        /// Convert a teacher model to teacher dto
        /// </summary>
        /// <param name="teacher">A teacher (full) model</param>
        /// <returns>Teacher Dto object, ready for Json serialization</returns>
        public static TeacherDto TeacherToTeacherDto(TeacherUser teacher)
        {
            return new TeacherDto()
            {
                TeacherId = teacher.Id,
                Name = teacher.FirstName + " " + teacher.LastName,
                Courses = teacher.Teachings?.Select(t => new TeacherDto.CourseList()
                {
                    Id = t.Course.Id,
                    Name = t.Course.Name
                }).ToList()
            };
        }

        /// <summary>
        /// Update full entity from dto before sending to the storage
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dto"></param>
        public static void UpdateTeachersPersonalData(TeacherUser user, TeacherUpdateDto dto)
        {
            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Gender = dto.Gender;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.Title = dto.Title;
            user.Degree = dto.Degree;
        }

        /// <summary>
        /// Convert a registration dto to full entity
        /// </summary>
        /// <param name="teacherReg"></param>
        /// <returns></returns>
        public static TeacherUser TeacherRegistrationDtoToTeacher(TeacherRegistrationDto teacherReg)
        {
            return new TeacherUser()
            {
                UserName = teacherReg.UserName,
                FirstName = teacherReg.FirstName,
                LastName = teacherReg.LastName,
                Gender = teacherReg.Gender,
                Email = teacherReg.Email,
                PhoneNumber = teacherReg.PhoneNumber
            };
        }

        /// <summary>
        /// Convert a teacher model to an extended data structure
        /// With a list of classrooms and courses
        /// and a list of courses and classrooms
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        public static TeacherExtendedDto TeacherToTeacherExtendedDto(TeacherUser teacher)
        {
            TeacherExtendedDto teacherData = new TeacherExtendedDto()
            {
                TeacherId = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                ClassRooms = teacher.Teachings
                    .SelectMany(t => t.Programs).GroupBy(p => p.ClassRoom)
                    .Select(g => new TeacherExtendedDto.ClassRoomCoursesDto()
                {
                    ClassRoomId = g.Key.Id,
                    ClassRoomName = g.Key.Name,
                    Grade = g.Key.ClassGrade,
                    Courses = g.Select(prog => new TeacherExtendedDto.ClassRoomCoursesDto.InnerCourseDto()
                    {
                        CourseId = prog.Course.Id,
                        CourseName = prog.Course.Name,
                        WeeklyHours = prog.WeeklyHours
                    }).ToList()
                }).ToList(),
                // No groupings or selectmanys?? and still working...
                Courses = teacher.Teachings
                    .Select(g => new TeacherExtendedDto.CourseClassRoomDto()
                {
                    CourseId = g.Course.Id,
                    CourseName = g.Course.Name,
                    ClassRooms = g.Programs.Select(p => new TeacherExtendedDto.CourseClassRoomDto.InnerClassRoomDto()
                    {
                        ClassRoomId = p.ClassRoom.Id,
                        ClassRoomName = p.ClassRoom.Name,
                        Grade = p.ClassRoom.ClassGrade,
                        WeeklyHours = p.WeeklyHours
                    }).ToList()
                }).ToList()
            };

            return teacherData;
        }

        /// <summary>
        /// Convert a teacher model to an extended data structure
        /// With a list of classrooms and courses
        /// and a list of courses and classrooms
        /// EXTRA and with a list of students nested inside...
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        public static TeacherExtendedEvenMoreDto TeacherToTeacherEvenMoreExtendedDto(TeacherUser teacher)
        {
            TeacherExtendedEvenMoreDto teacherData = new TeacherExtendedEvenMoreDto()
            {
                TeacherId = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                ClassRooms = teacher.Teachings
                    .SelectMany(t => t.Programs).GroupBy(p => p.ClassRoom)
                    .Select(g => new TeacherExtendedEvenMoreDto.ClassRoomCoursesDto()
                    {
                        ClassRoomId = g.Key.Id,
                        ClassRoomName = g.Key.Name,
                        Grade = g.Key.ClassGrade,
                        Courses = g.Select(prog => new TeacherExtendedEvenMoreDto.ClassRoomCoursesDto.InnerCourseDto()
                        {
                            CourseId = prog.Course.Id,
                            CourseName = prog.Course.Name,
                            WeeklyHours = prog.WeeklyHours,
                            Students = prog.TakingStudents.Select(s => new TeacherExtendedEvenMoreDto.ClassRoomCoursesDto.InnerCourseDto.StudentDto()
                            {
                                StudentId = s.Student.Id,
                                FirstName = s.Student.FirstName,
                                LastName = s.Student.LastName
                            })
                        }).ToList()
                    }).ToList(),
                // No groupings or selectmanys?? and still working...
                Courses = teacher.Teachings
                    .Select(g => new TeacherExtendedEvenMoreDto.CourseClassRoomDto()
                    {
                        CourseId = g.Course.Id,
                        CourseName = g.Course.Name,
                        ClassRooms = g.Programs.Select(p => new TeacherExtendedEvenMoreDto.CourseClassRoomDto.InnerClassRoomDto()
                        {
                            ClassRoomId = p.ClassRoom.Id,
                            ClassRoomName = p.ClassRoom.Name,
                            Grade = p.ClassRoom.ClassGrade,
                            WeeklyHours = p.WeeklyHours,
                            Students = p.TakingStudents.Select(s => new TeacherExtendedEvenMoreDto.CourseClassRoomDto.InnerClassRoomDto.StudentDto()
                            {
                                StudentId = s.Student.Id,
                                FirstName = s.Student.FirstName,
                                LastName = s.Student.LastName
                            })
                        }).ToList()
                    }).ToList()
            };

            return teacherData;
        }
    }
}