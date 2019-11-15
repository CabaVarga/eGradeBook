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
        #region CRUD Entities
        /// <summary>
        /// Create a teaching from TeachingDto
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        Teaching CreateTeaching(TeachingDto teachingDto);

        /// <summary>
        /// Create a teaching from a courseId and a teacherId
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Teaching CreateTeaching(int courseId, int teacherId);

        /// <summary>
        /// Get a teaching for courseId and teacherId
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Teaching GetTeaching(int courseId, int teacherId);

        /// <summary>
        /// Get a teaching by teachingId
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        Teaching GetTeachingById(int teachingId);

        /// <summary>
        /// Get all teachings
        /// </summary>
        /// <returns></returns>
        IEnumerable<Teaching> GetAllTeachings();

        /// <summary>
        /// Delete teaching based on supplied teachingDto
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        Teaching DeleteTeaching(TeachingDto teachingDto);

        /// <summary>
        /// Delete teaching by courseId and teacherId
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        Teaching DeleteTeaching(int courseId, int teacherId);
        #endregion

        #region CRUD Dtos
        /// <summary>
        /// Create a teaching from TeachingDto, return TeachingDto
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        TeachingDto CreateTeachingDto(TeachingDto teachingDto);

        /// <summary>
        /// Get all teachings as dtos
        /// </summary>
        /// <returns></returns>
        IEnumerable<TeachingDto> GetAllTeachingsDtos();

        /// <summary>
        /// Get teaching by teachingId, return teachingDto
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        TeachingDto GetTeachingDtoById(int teachingId);
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

        /// <summary>
        /// Get all teachings for a course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        IEnumerable<Teaching> GetAllTeachingsForCourse(int courseId);

        /// <summary>
        /// Get all teaching for a teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        IEnumerable<Teaching> GetAllTeachingsForTeacher(int teacherId);

        /// <summary>
        /// Delete Teaching by teachingId
        /// </summary>
        /// <param name="teachingId"></param>
        /// <returns></returns>
        TeachingDto DeleteTeaching(int teachingId);

        /// <summary>
        /// Get Teachings by courseId or teacherId
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        IEnumerable<TeachingDto> GetTeachingsByParameters(int? courseId, int? teacherId);

        IEnumerable<TeachingDto> GetTeachingsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}