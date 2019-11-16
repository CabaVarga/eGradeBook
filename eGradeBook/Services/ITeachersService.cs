using eGradeBook.Models;
using eGradeBook.Models.Dtos.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with teachers data and related tasks
    /// </summary>
    public interface ITeachersService
    {
        // These will must go
        /// <summary>
        /// Retrieve all teachers
        /// </summary>
        /// <returns></returns>
        IEnumerable<TeacherDto> GetAllTeachers();

        /// <summary>
        /// Retrieve a teacher by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TeacherDto GetTeacherById(int id);

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
        Task<TeacherDto> DeleteTeacher(int teacherId);




        IEnumerable<TeacherDto> GetTeachersByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}