using eGradeBook.Models;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Models.Dtos.Takings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Interface for students related tasks
    /// </summary>
    public interface IStudentsService
    {
        /// <summary>
        /// Retrieve students by their first name starting with
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        IEnumerable<StudentDto> GetStudentsByFirstNameStartingWith(string start);

        /// <summary>
        /// Retrieve students by their last name starting with
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        IEnumerable<StudentDto> GetStudentsByLastNameStartingWith(string start);

        /// <summary>
        /// Update a student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        StudentDto UpdateStudent(int studentId, StudentDto student);

        /// <summary>
        /// Delete a student
        /// TODO what should I do with these entries? If I'm using Identity for changes?
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        Task<StudentDto> DeleteStudent(int studentId);

        /// <summary>
        /// Retrieve all students
        /// </summary>
        /// <returns></returns>
        IEnumerable<StudentDto> GetAllStudents();

        StudentDto GetStudentById(int studentId);

        IEnumerable<StudentDto> GetStudentsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}