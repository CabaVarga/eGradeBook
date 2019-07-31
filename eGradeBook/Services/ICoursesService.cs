using eGradeBook.Models;
using eGradeBook.Models.Dtos;
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
        // FULL MODEL CRUD

        /// <summary>
        /// Get all courses
        /// Full model
        /// </summary>
        /// <returns></returns>
        IEnumerable<Course> GetAllCourses();

        /// <summary>
        /// Get the course by Id
        /// Full model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Course GetCourseById(int id);

        /// <summary>
        /// Create a course
        /// Full model
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        Course CreateCourse(Course course);

        /// <summary>
        /// Update a course
        /// Full model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        Course UpdateCourse(int id, Course course);

        /// <summary>
        /// Delete a course by Id
        /// Returns full model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Course DeleteCourse(int id);

        // Admin Course Management by Dtos
        
        /// <summary>
        /// Create a course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        CourseDto CreateCourse(CourseDto course);

        /// <summary>
        /// Update a course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        CourseDto UpdateCourse(CourseDto course);

        /// <summary>
        /// Delete a course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        CourseDto DeleteCourse(CourseDto course);

        // Assign and remove teacher
        /// <summary>
        /// Assign a teacher to the course
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        void AssignTeacherToCourse(int teacherId, int courseId);

        /// <summary>
        /// Remove a teacher from the course
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        void RemoveTeacherFromCourse(int teacherId, int courseId);

        // TODO remove full models, add Dtos
    }
}