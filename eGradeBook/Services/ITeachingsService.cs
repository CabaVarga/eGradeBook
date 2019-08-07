using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Teachings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with Teaching entities.
    /// Teaching entities are connecting teachers and courses.
    /// NOTE do they need a special service or I should handle them through
    /// TeachersService and CoursesService?
    /// Or both, but other services using Teachings service when they are needing its' services?
    /// </summary>
    public interface ITeachingsService
    {
        #region Full CRUD
        Teaching CreateTeaching(TeachingDto teachingDto);
        Teaching CreateTeaching(int courseId, int teacherId);
        Teaching GetTeaching(TeachingDto teachingDto);
        Teaching GetTeaching(int courseId, int teacherId);
        Teaching DeleteTeaching(TeachingDto teachingDto);
        Teaching DeleteTeaching(int courseId, int teacherId);
        #endregion
        /// <summary>
        /// Retrieve a list of teaching assignments grouped by courses
        /// </summary>
        /// <returns></returns>
        IEnumerable<TeachingsByCoursesDto> GetAllTeachingAssignmentsByCourses();

        /// <summary>
        /// Retrieve a list of teaching assignments grouped by teachers
        /// </summary>
        /// <returns></returns>
        IEnumerable<TeachingsByTeachersDto> GetAllTeachingAssignmentsByTeachers();

        /// <summary>
        /// Assign a teacher to a course
        /// NOTE already have an implementation at teacher...
        /// </summary>
        /// <param name="courseId">Course Id</param>
        /// <param name="teacherId">Teacher Id</param>
        /// <returns></returns>
        Teaching AssignTeacherToCourse(int courseId, int teacherId);

        /// <summary>
        /// Remove a teacher from a course
        /// </summary>
        /// <param name="courseId">Course Id</param>
        /// <param name="teacherId">Teacher Id</param>
        /// <returns></returns>
        Teaching RemoveTeacherFromCourse(int courseId, int teacherId);
    }
}