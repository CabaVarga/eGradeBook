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
        // FULL MODEL
        StudentParent CreateStudentParent(StudentParentDto studentParentDto);
        StudentParent CreateStudentParent(int studentId, int parentId);

        StudentParent GetStudentParent(StudentParentDto studentParentDto);
        StudentParent GetStudentParent(int studentId, int parentId);
        StudentParent GetStudentParent(int studentParentId);

        IEnumerable<StudentParent> GetAllStudentParents();

        StudentParent DeleteStudentParent(StudentParentDto studentParentDto);
        StudentParent DeleteStudentParent(int studentId, int parentId);
        StudentParent DeleteStudentParent(int studentParentId);

        // DTO
        StudentParentDto CreateStudentParentDto(StudentParentDto studentParentDto);
        StudentParentDto CreateStudentParentDto(int studentId, int parentId);

        StudentParentDto GetStudentParentDto(StudentParentDto studentParentDto);
        StudentParentDto GetStudentParentDto(int studentId, int parentId);
        StudentParentDto GetStudentParentDto(int studentParentId);

        IEnumerable<StudentParentDto> GetAllStudentParentsDto();

        StudentParentDto DeleteStudentParentDto(StudentParentDto studentParentDto);
        StudentParentDto DeleteStudentParentDto(int studentId, int parentId);
        StudentParentDto DeleteStudentParentDto(int studentParentId);

        // Diretct access is problematic...
    }
}