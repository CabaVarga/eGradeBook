using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Courses;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Models.Dtos.Teachings;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
using eGradeBook.Services.Exceptions;
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
        /// Delete course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseDto DeleteCourse(int id)
        {
            Course deletedCourse = db.CoursesRepository.GetByID(id);

            if (deletedCourse == null)
            {
                return null;
            }


            db.CoursesRepository.Delete(deletedCourse);
            db.Save();

            return Converters.CoursesConverter.CourseToCourseDto(deletedCourse);
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


            db.CoursesRepository.Insert(newCourse);
            db.Save();

            CourseDto courseDto = new CourseDto()
            {
                Name = newCourse.Name,
                ColloqialName = newCourse.ColloqialName,
                CourseId = newCourse.Id
            };

            logger.Info("Course creation succesful");
            return courseDto;
        }


        /// <summary>
        /// Update course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public CourseDto UpdateCourse(CourseDto course)
        {
            Course updatedCourse = db.CoursesRepository.GetByID(course.CourseId);

            updatedCourse.Name = course.Name;
            updatedCourse.ColloqialName = course.ColloqialName;

            db.CoursesRepository.Update(updatedCourse);
            db.Save();

            return CoursesConverter.CourseToCourseDto(updatedCourse);
        }

        /// <summary>
        /// Retrieve all courses and return them as CourseDtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CourseDto> GetAllCourses()
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
        public CourseDto GetCourseById(int courseId)
        {
            logger.Trace("Service received a request to a course by Id {courseId}", courseId);
            var course = db.CoursesRepository.GetByID(courseId);

            if (course == null)
            {
                return null;
            }

            return CoursesConverter.CourseToCourseDto(course);
        }

        public IEnumerable<CourseDto> GetCoursesByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var courses = db.CoursesRepository.Get(
                g => (courseId != null ? g.Id == courseId : true) &&
                    (teacherId != null ? g.Teachings.Any(t => t.TeacherId == teacherId) : true) &&
                    (classRoomId != null ? g.Teachings.Any(t => t.Programs.Any(p => p.ClassRoomId == classRoomId)) : true) &&
                    (studentId != null ? g.Teachings.Any(t => t.Programs.Any(p => p.Takings.Any(tk => tk.Enrollment.StudentId == studentId))) : true) &&
                    (parentId != null ? g.Teachings.Any(t => t.Programs.Any(p => p.Takings.Any(tk => tk.Enrollment.Student.StudentParents.Any(sp => sp.ParentId == parentId)))) : true) &&
                    (schoolGrade != null ? g.Teachings.Any(t => t.Programs.Any(p => p.ClassRoom.ClassGrade == schoolGrade)) : true))
                    .Select(g => Converters.CoursesConverter.CourseToCourseDto(g));

            return courses;
        }
    }
}