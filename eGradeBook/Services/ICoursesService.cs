using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Courses;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Models.Dtos.Teachings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Interface for Courses service
    /// </summary>
    public interface ICoursesService
    {
        
        /// <summary>
        /// Create a course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        CourseDto CreateCourse(CourseDto course);

        /// <summary>
        /// Get all Courses as Dtos
        /// </summary>
        /// <returns></returns>
        IEnumerable<CourseDto> GetAllCourses();

        /// <summary>
        /// Get a Course Dto by Id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        CourseDto GetCourseById(int courseId);

        
        /// <summary>
        /// Update a course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        CourseDto UpdateCourse(CourseDto course);

        CourseDto DeleteCourse(int courseId);

        IEnumerable<CourseDto> GetCoursesByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}