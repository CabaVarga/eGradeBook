using eGradeBook.Models;
using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Interface for parents service
    /// </summary>
    public interface IParentsService
    {
        /// <summary>
        /// Get all parents with their children
        /// </summary>
        /// <returns></returns>
        IEnumerable<ParentChildrenDto> GetAllParentsWithTheirChildrent();

        /// <summary>
        /// Get all children of a given parent
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        IEnumerable<StudentDto> GetAllChildren(int parentId);

        /// <summary>
        /// Connect a student with a parent
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        ParentChildrenDto AddChild(int parentId, int studentId);

        // THIS WILL GO TO STUDENTS BUT I WANT TO TEST OUT...

        /// <summary>
        /// Get parents by students
        /// </summary>
        /// <returns></returns>
        IEnumerable<StudentParentsDto> GetParentsForStudents();

        /// <summary>
        /// Get parents of a given student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentParentsDto GetParentsForStudent(int studentId);

        // CRUD without the C

        /// <summary>
        /// Get parent by Id
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        ParentUser GetParentById(int parentId);

        ParentDto GetParentByIdDto(int parentId);

        ParentReportDto GetParentReport(int parentId);

        /// <summary>
        /// Get all parent users
        /// </summary>
        /// <returns></returns>
        IEnumerable<ParentDto> GetAllParents();

        Task<ParentDto> DeleteParent(int parentId);

        IEnumerable<ParentDto> GetParentsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}