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
        /// Get all Courses as Dtos
        /// </summary>
        /// <returns></returns>
        IEnumerable<CourseDto> GetAllCoursesDto();

        /// <summary>
        /// Get a Course Dto by Id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        CourseDto GetCourseDtoById(int courseId);

        
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

        #region Teaching -- a course + teacher association
        TeachingDto GetTeaching(int courseId, int teacherId);
        IEnumerable<TeachingDto> GetAllTeachings(int courseId);

        TeachingDto CreateTeachingAssignment(TeachingDto teaching);
        TeachingDto DeleteTeachingAssignment(TeachingDto teaching);

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
        #endregion


        // TODO remove full models, add Dtos
        // For creation and everything else, it can work as simply adding the resource (teacher, classroom and weekly hours, etc.. )

        #region Programs -- a Teaching (course and teacher) + ClassRoom association
        IEnumerable<ProgramDto> GetAllPrograms(int courseId, int teacherId);
        ProgramDto GetProgram(int courseId, int teacherId, int classRoomId);

        ProgramDto CreateProgram(ProgramDto program);
        ProgramDto UpdateProgram(ProgramDto program); // only the weekly hours can be updated...
        ProgramDto DeleteProgram(ProgramDto program);
        #endregion

        #region Takings -- A Program (course, teacher and classroom) + student association
        IEnumerable<TakingDto> GetAllTakings(int courseId, int teacherId, int classRoomId);
        TakingDto GetTaking(int courseId, int teacherId, int classRoomId, int studentId);

        TakingDto CreateTaking(TakingDto taking);
        TakingDto DeleteTaking(TakingDto taking);
        #endregion

        IEnumerable<CourseDto> GetCoursesByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}