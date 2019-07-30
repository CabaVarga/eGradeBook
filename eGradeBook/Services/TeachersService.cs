using eGradeBook.Models;
using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public class TeachersService : ITeachersService
    {
        private IUnitOfWork db;
        private ITeachingsService teachingsService;

        public TeachersService(IUnitOfWork db, ITeachingsService teachingsService)
        {
            this.db = db;
            this.teachingsService = teachingsService;
        }

        public void AssignCourseToTeacher(TeachingAssignmentDto assignment)
        {
            teachingsService.AssignTeacherToCourse(assignment.SubjectId, assignment.TeacherId);
        }

        public IEnumerable<TeacherDto> GetAllTeachersDtos()
        {
            return db.TeachersRepository.Get()
                .OfType<TeacherUser>()
                .Select(t => new TeacherDto()
                {
                    Name = t.FirstName + " " + t.LastName,
                    TeacherId = t.Id,
                    Courses = t.Teachings.Select(tc => new TeacherDto.CourseList() { Id = tc.SubjectId, Name = tc.Course.Name }).ToList()
//                    Courses = t.Teachings.Select(tc => tc.Course.Name).ToList()
                });
        }

        public TeacherDto GetTeacherByIdDto(int id)
        {
            //            return db.TeachersRepository.Get(t => t.Id == id)
            //                .OfType<TeacherUser>()
            //                .Select(t => new TeacherDto()
            //                {
            //                    Name = t.FirstName + " " + t.LastName,
            //                    TeacherId = t.Id,
            //                    Courses = t.Teachings.Select(tc => new TeacherDto.CourseList() { Id = tc.SubjectId, Name = tc.Course.Name }).ToList()
            ////                    Courses = t.Teachings.Select(tc => tc.Course.Name).ToList()
            //                })
            //                .FirstOrDefault();

            var t = db.TeachersRepository.GetByID(id);


            var dto = new TeacherDto()
            {
                Name = t.FirstName + " " + t.LastName,
                TeacherId = t.Id,
                Courses = t.Teachings.Select(tc => new TeacherDto.CourseList() { Id = tc.SubjectId, Name = tc.Course.Name }).ToList()
                //                    Courses = t.Teachings.Select(tc => tc.Course.Name).ToList()
            };

            return dto;

        }
    }
}