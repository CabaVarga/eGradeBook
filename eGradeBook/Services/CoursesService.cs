using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Courses;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with courses and all tasks related to them
    /// </summary>
    public class CoursesService : ICoursesService
    {
        private IUnitOfWork db;
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public CoursesService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        /// <summary>
        /// Create a new course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public Course CreateCourse(Course course)
        {
            db.CoursesRepository.Insert(course);
            db.Save();

            return course;
        }

        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Course> GetAllCourses()
        {
            return db.CoursesRepository.Get();
        }

        /// <summary>
        /// Get a course by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Course GetCourseById(int id)
        {
            Course course = db.CoursesRepository.GetByID(id);

            if (course == null)
            {
                return null;
            }

            return course;
        }

        /// <summary>
        /// Update course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="course"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Delete course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Create course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public CourseDto CreateCourse(CourseDto course)
        {
            logger.Info("Service received request for Course {@courseData} creation", course);

            Course newCourse = new Course()
            {
                Name = course.Name,
                ColloqialName = course.ColloqialName
            };

            // NOTE Should I inspect beforehand or just shoot
            // and handle error in case of unsuccessful insertion
            CreateCourse(newCourse);

            CourseDto courseDto = new CourseDto()
            {
                Name = newCourse.Name,
                ColloqialName = newCourse.ColloqialName,
                Id = newCourse.Id
            };

            logger.Info("Course creation succesful");
            return courseDto;
        }

        /// <summary>
        /// Delete course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public CourseDto DeleteCourse(CourseDto course)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public CourseDto UpdateCourse(CourseDto course)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Assign teacher to course
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        public void AssignTeacherToCourse(int teacherId, int courseId)
        {
            var user = db.TeachersRepository.GetByID(teacherId);

            if (user.GetType() != typeof(TeacherUser))
            {
                //
            }

            // ifnull also...
        }

        /// <summary>
        /// Remove teacher from course
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        public void RemoveTeacherFromCourse(int teacherId, int courseId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve all courses and return them as CourseDtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CourseDto> GetAllCoursesDto()
        {
            logger.Trace("Service received a request to return all courses as dtos");
            return db.CoursesRepository.Get()
                .Select(c => CoursesConverter.CourseToCourseDto(c));
        }

        /// <summary>
        /// Retrieve a course by Id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public CourseDto GetCourseDtoById(int courseId)
        {
            logger.Trace("Service received a request to a course by Id {courseId}", courseId);
            var course = db.CoursesRepository.GetByID(courseId);

            if (course == null)
            {
                return null;
            }

            return CoursesConverter.CourseToCourseDto(course);
        }
    }
}