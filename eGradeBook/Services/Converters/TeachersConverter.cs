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
                UserName = teacher.UserName,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                Gender = teacher.Gender,
                Email = teacher.Email,
                Phone = teacher.PhoneNumber,
                Degree = teacher.Degree,
                Title = teacher.Title
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
                PhoneNumber = teacherReg.PhoneNumber,
                Degree = teacherReg.Degree,
                Title = teacherReg.Title
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
                                // This was not throwing because I used the include, duh!
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

        public static TeacherReportDto TeacherToTeacherReportDto(TeacherUser teacher)
        {
            TeacherReportDto report = new TeacherReportDto()
            {
                TeacherId = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                ClassRooms = teacher.Teachings
                    .SelectMany(t => t.Programs).GroupBy(p => p.ClassRoom)
                    .Select(g => ClassRoomProgramGroupingToTeacherReportClassRoom(g)),
                Courses = teacher.Teachings.Select(t => TeachingToTeacherReportCourse(t))
            };

            return report;
        }

        private static TeacherReportDto.ClassRoomDto ClassRoomProgramGroupingToTeacherReportClassRoom(IGrouping<ClassRoom, Program> grouping)
        {
            return new TeacherReportDto.ClassRoomDto()
            {
                ClassRoomId = grouping.Key.Id,
                ClassRoomName = grouping.Key.Name,
                Grade = grouping.Key.ClassGrade,
                Courses = grouping.Select(program => ProgramToTeacherReportClassRoomCourse(program))
            };
        }

        private static TeacherReportDto.ClassRoomDto.CourseDto ProgramToTeacherReportClassRoomCourse(Program program)
        {
            return new TeacherReportDto.ClassRoomDto.CourseDto()
            {
                CourseId = program.Course.Id,
                CourseName = program.Course.Name,
                WeeklyHours = program.WeeklyHours,
                Students = program.TakingStudents.Select(taking => TakingToStudent(taking))
            };
        }

        private static TeacherReportDto.StudentDto TakingToStudent(Taking taking)
        {
            return new TeacherReportDto.StudentDto()
            {
                StudentId = taking.Student.Id,
                FirstName = taking.Student.FirstName,
                LastName = taking.Student.LastName,
                Grades = taking.Grades.Select(g => GradeToStudentGrade(g)).ToList(),
                Parents = taking.Student.StudentParents.Select(sp => StudentParentToStudentParent(sp))
            };
        }

        private static TeacherReportDto.StudentDto.GradeDto GradeToStudentGrade(Grade grade)
        {
            return new TeacherReportDto.StudentDto.GradeDto()
            {
                GradeId = grade.Id,
                GradePoint = grade.GradePoint,
                Assigned = grade.Assigned,
                Semester = grade.SchoolTerm,
                Notes = grade.Notes
            };
        }

        private static TeacherReportDto.StudentDto.ParentDto StudentParentToStudentParent(StudentParent studentParent)
        {
            return new TeacherReportDto.StudentDto.ParentDto()
            {
                ParentId = studentParent.Parent.Id,
                FirstName = studentParent.Parent.FirstName,
                LastName = studentParent.Parent.LastName
            };
        }

        private static TeacherReportDto.CourseDto TeachingToTeacherReportCourse(Teaching teaching)
        {
            return new TeacherReportDto.CourseDto()
            {
                CourseId = teaching.Course.Id,
                CourseName = teaching.Course.Name,
                ClassRooms = teaching.Programs.Select(p => ProgramToTeacherReportCourseClassRoom(p))
            };
        }

        private static TeacherReportDto.CourseDto.ClassRoomDto ProgramToTeacherReportCourseClassRoom(Program program)
        {
            return new TeacherReportDto.CourseDto.ClassRoomDto()
            {
                ClassRoomId = program.ClassRoom.Id,
                ClassRoomName = program.ClassRoom.Name,
                Grade = program.ClassRoom.ClassGrade,
                WeeklyHours = program.WeeklyHours,
                Students = program.TakingStudents.Select(t => TakingToStudent(t))
            };
        }
    }
}