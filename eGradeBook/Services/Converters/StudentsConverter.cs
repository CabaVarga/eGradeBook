using eGradeBook.Models;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    /// <summary>
    /// Converters for student users and their relations
    /// </summary>
    public static class StudentsConverter
    {
        /// <summary>
        /// Convert a student model to student dto
        /// </summary>
        /// <param name="student">A student (full) model</param>
        /// <returns>Student Dto object, ready for Json serialization</returns>
        public static StudentDto StudentToStudentDto(StudentUser student)
        {
            return new StudentDto()
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                PlaceOfBirth = student.PlaceOfBirth,
                DateOfBirth = student.DateOfBirth,
                ClassRoom = student.ClassRoom?.Name,
                ClassRoomId = student.ClassRoomId
            };
        }

        /// <summary>
        /// Convert registration dto to full entity
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public static StudentUser StudentRegistrationDtoToStudent(StudentRegistrationDto dto)
        {
            return new StudentUser()
            {
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Gender = dto.Gender,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PlaceOfBirth = dto.PlaceOfBirth,
                DateOfBirth = dto.DateOfBirth
            };
        }

        /// <summary>
        /// Update full entity from dto before sending to the storage
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dto"></param>
        public static void UpdateStudentsPersonalData(StudentUser user, StudentUpdateDto dto)
        {
            user.UserName = dto.UserName;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Gender = dto.Gender;
            user.Email = dto.Email;
            user.PhoneNumber = dto.PhoneNumber;
            user.PlaceOfBirth = dto.PlaceOfBirth;
            user.DateOfBirth = dto.DateOfBirth;
        }

        /// <summary>
        /// Convert a student model to student with parents dto
        /// </summary>
        /// <param name="student">A student (full) model</param>
        /// <returns>Student Dto object, ready for Json serialization</returns>
        public static StudentWithParentsDto StudentToStudentWithParentsDto(StudentUser student)
        {
            return new StudentWithParentsDto()
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                ClassRoom = student.ClassRoom.Name,
                ClassRoomId = student.ClassRoomId,
                Parents = student.StudentParents.Select(sp => new ParentsDto()
                {
                    FirstName = sp.Parent.FirstName,
                    LastName = sp.Parent.LastName,
                    ParentId = sp.ParentId
                })
            };
        }

        /// <summary>
        /// Create grade report for student
        /// </summary>
        /// <param name="studentId"></param>
        public static StudentReportDto StudentToStudentReportDto(StudentUser student)
        {
            var report = new StudentReportDto()
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                PlaceOfBirth = student.PlaceOfBirth,
                DateOfBirth = student.DateOfBirth,
                ClassRoom = new StudentReportDto.ClassRoomDto()
                {
                    ClassRoomId = student.ClassRoom?.Id,
                    Name = student.ClassRoom?.Name,
                    SchoolGrade = student.ClassRoom?.ClassGrade
                },
                Courses = student.Takings.Select(t => new StudentReportDto.CoursesDto()
                {
                    CourseId = t.Program.Teaching.Course.Id,
                    CourseName = t.Program.Teaching.Course.Name,
                    Teacher = new StudentReportDto.CoursesDto.TeacherDto()
                    {
                        TeacherId = t.Program.Teaching.Teacher.Id,
                        FirstName = t.Program.Teaching.Teacher.FirstName,
                        LastName = t.Program.Teaching.Teacher.LastName
                    }
                }),
                Teachers = student.Takings.Select(t => new StudentReportDto.TeacherDto()
                {
                    TeacherId = t.Program.Teaching.Teacher.Id,
                    FirstName = t.Program.Teaching.Teacher.FirstName,
                    LastName = t.Program.Teaching.Teacher.LastName,
                    Courses = student.Takings
                        .Where(tk => tk.Program.Teaching.Teacher.Id == t.Program.Teaching.Teacher.Id)
                        .Select(tkg => new StudentReportDto.TeacherDto.CourseDto()
                        {
                            CourseId = tkg.Program.Teaching.Course.Id,
                            CourseName = tkg.Program.Teaching.Course.Name
                        })
                }).Distinct(),
                Grades = student.Takings.SelectMany(t => t.Grades).Select(g => new StudentReportDto.GradeDto()
                {
                    GradeId = g.Id,
                    CourseId = g.Taking.Program.Teaching.Course.Id,
                    TeacherId = g.Taking.Program.Teaching.Teacher.Id,
                    ClassRoomId = g.Taking.Program.ClassRoom.Id,
                    StudentId = g.Taking.Student.Id,
                    GradePoint = g.GradePoint,
                    AssignmentDate = g.Assigned,
                    SchoolTerm = g.SchoolTerm,
                    Notes = g.Notes
                }),
                Parents = student.StudentParents.Select(sp => new StudentReportDto.ParentDto()
                {
                    ParentId = sp.Parent.Id,
                    FirstName = sp.Parent.FirstName,
                    LastName = sp.Parent.LastName
                })
            };

            return report;
        }
    }
}