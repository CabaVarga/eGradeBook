using eGradeBook.Models;
using eGradeBook.Models.Dtos.StudentParents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface IStudentParentsService
    {

        StudentParentDto CreateStudentParent(StudentParentDto studentParentDto);

        StudentParentDto GetStudentParentById(int studentParentId);

        IEnumerable<StudentParentDto> GetAllStudentParents();

        // Diretct access is problematic...
        StudentParentDto DeleteStudentParentForReal(int studentParentId);

        IEnumerable<StudentParentDto> GetStudentParentsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}