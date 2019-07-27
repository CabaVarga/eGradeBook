using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public class CoursesService : ICoursesService
    {
        private IUnitOfWork db;

        public CoursesService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return db.CoursesRepository.Get();
        }

        public Course GetCourseById(int id)
        {
            Course course = db.CoursesRepository.GetByID(id);

            if (course == null)
            {
                return null;
            }

            return course;
        }

        public CourseDto UpdateCourse(CourseDto course)
        {
            throw new NotImplementedException();
        }

        public Course CreateCourse(Course course)
        {
            db.CoursesRepository.Insert(course);
            db.Save();

            return course;
        }

        public Course UpdateCourse(int id, Course course)
        {
            Course courseUpdated = db.CoursesRepository.GetByID(id);

            if (courseUpdated == null)
            {
                return null;
            }

            courseUpdated.Name = course.Name;
            courseUpdated.ColloqialName = course.ColloqialName;

            db.CoursesRepository.Update(courseUpdated);
            db.Save();

            return courseUpdated;
        }

        public Course DeleteCourse(int id)
        {
            Course deletedCourse = db.CoursesRepository.GetByID(id);

            if (deletedCourse == null)
            {
                return null;
            }

            db.CoursesRepository.Delete(deletedCourse);
            db.Save();

            return deletedCourse;
        }
        public CourseDto CreateCourse(CourseDto course)
        {
            Course newCourse = new Course()
            {
                Name = course.Name,
                ColloqialName = course.ColloqialName
            };

            CreateCourse(newCourse);

            CourseDto courseDto = new CourseDto()
            {
                Name = newCourse.Name,
                ColloqialName = newCourse.ColloqialName,
                Id = newCourse.Id
            };

            return courseDto;
        }
        public CourseDto DeleteCourse(CourseDto course)
        {
            throw new NotImplementedException();
        }

        public void AssignTeacherToCourse(int teacherId, int courseId)
        {
            var user = db.TeachersRepository.GetByID(teacherId);

            if (user.GetType() != typeof(TeacherUser))
            {
                //
            }

            // ifnull also...
        }

        public void RemoveTeacherFromCourse(int teacherId, int courseId)
        {
            throw new NotImplementedException();
        }
    }
}