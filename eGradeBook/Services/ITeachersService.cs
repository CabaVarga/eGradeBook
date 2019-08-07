using eGradeBook.Models;
using eGradeBook.Models.Dtos.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with teachers data and related tasks
    /// </summary>
    public interface ITeachersService
    {
        #region Full CRUD --- well, only R, even that incomplete...
        TeacherUser GetTeacherById(int teacherId);
        #endregion
        // These will must go
        /// <summary>
        /// Retrieve all teachers
        /// </summary>
        /// <returns></returns>
        IEnumerable<TeacherDto> GetAllTeachersDtos();

        /// <summary>
        /// Retrieve a teacher by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TeacherDto GetTeacherByIdDto(int id);

        /// <summary>
        /// Assign a course to a teacher
        /// </summary>
        /// <param name="assignment"></param>
        void AssignCourseToTeacher(TeachingAssignmentDto assignment);

        // CRUD without the C
        /// <summary>
        /// Retrieve all teachers
        /// </summary>
        /// <returns></returns>
        IEnumerable<TeacherDto> GetAllTeachers();

        /// <summary>
        /// Update a teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="teacher"></param>
        /// <returns></returns>
        TeacherDto UpdateTeacher(int teacherId, TeacherDto teacher);

        /// <summary>
        /// Delete a teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        TeacherDto DeleteTeacher(int teacherId);

        TeacherExtendedDto GetExtendedDataForTeacher(int teacherId);

        // MUST TEST THESE

        object GetClassRoomsForTeacher(int teacherId);
        object GetCoursesForTeacher(int teacherId);
        object GetClassRoomsCoursesForTeacher(int teacherId);
        object GetCoursesClassRoomsForTeacher(int teacherId);
    }
}