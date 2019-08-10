using eGradeBook.Models;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Models.Dtos.Takings;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // This will have to be gone
        /// <summary>
        /// Retrieve all students
        /// </summary>
        /// <returns></returns>
        IEnumerable<StudentUser> GetAllStudents();

        // Course GPA Dto GetGpaForStudentCourse(int studentId, int courseId);
        // Student Courses GPA (by course and total) GetGpaForStudent(int studentId)

        // CRUD without the C
        /// <summary>
        /// Get a student by Id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentUser GetStudentById(int studentId);

        // IEnumerable<StudentDto> GetAllStudents();

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
        StudentDto DeleteStudent(int studentId);

        /// <summary>
        /// Retrieve all students
        /// </summary>
        /// <returns></returns>
        IEnumerable<StudentDto> GetAllStudentsDto();

        /// <summary>
        /// Retrieve all students and their parents
        /// </summary>
        /// <returns></returns>
        IEnumerable<StudentWithParentsDto> GetAllStudentsWithParents();

        /// <summary>
        /// Assign a course to a student
        /// NOTE I think I added this in a hurry.
        /// Need to rethink this implementation... on the other hand, maybe it's enough?
        /// We already know the classRoom and who teaches the course in the classroom...
        /// Is the void an ok return type?
        /// Probably a Full student dto
        /// Or at least student + courses list would be maybe better?
        /// </summary>
        /// <param name="course"></param>
        TakingDto AssignCourseToStudent(StudentCourseDto course);

        StudentDto GetStudentByIdDto(int studentId);

        StudentReportDto GetStudentReport(int studentId);

        bool IsParent(int studentId, int parentId);

        IEnumerable<StudentDto> GetStudentsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}