using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core;
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

        public Teaching AssignTeacherToCourse(int courseId, int teacherId)
        {
            var course = db.CoursesRepository.GetByID(courseId);

            if (course == null) return null;

            var teacher = db.TeachersRepository.GetByID(teacherId);

            if (teacher == null) return null;

            try
            {
                var teaching = new Teaching()
                {
                    Course = course,
                    Teacher = teacher
                };

                db.TeachingAssignmentsRepository.Insert(teaching);
                db.Save();

                return teaching;
            }

            catch (EntityException ex)
            {
                // entity framework base exception
                return null;
            }

            catch (DbException ex)
            {
                // generic abstract data source exception
                return null;
            }
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

        public Teaching RemoveTeacherFromCourse(int courseId, int teacherId)
        {
            // I need the id's on the model, easier to work with them especially on link tables.
            return null;
        }
    }
}