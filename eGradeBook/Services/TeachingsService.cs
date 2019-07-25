using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public class TeachingsService : ITeachingsService
    {
        private IUnitOfWork db;

        public TeachingsService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<TeachingsByCoursesDto> GetAllTeachingAssignmentsByCourses()
        {
            var teachings = db.TeachingAssignmentsRepository.Get()
                .GroupBy(ta => ta.Course)
                .Select(ta => new TeachingsByCoursesDto()
                {
                    Course = ta.Key.ColloqialName,
                    Teachers = ta.Select(c => c.Teacher.UserName).OrderBy(t => t).ToList()
                });

            return teachings;
        }

        public IEnumerable<TeachingsByTeachersDto> GetAllTeachingAssignmentsByTeachers()
        {
            var teachings = db.TeachingAssignmentsRepository.Get()
                .GroupBy(ta => ta.Teacher)
                .Select(ta => new TeachingsByTeachersDto()
                {
                    Teacher = ta.Key.UserName,
                    Courses = ta.Select(c => c.Course.ColloqialName).OrderBy(t => t).ToList()
                });

            return teachings;
        }
    }
}