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
        ParentDto GetParentById(int parentId);

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