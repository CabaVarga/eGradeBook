using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface IParentsService
    {
        IEnumerable<ParentChildrenDto> GetAllParentsWithTheirChildrent();
        IEnumerable<StudentDto> GetAllChildren(int parentId);
        ParentChildrenDto AddChild(int parentId, int studentId);

        // THIS WILL GO TO STUDENTS BUT I WANT TO TEST OUT...
        IEnumerable<StudentParentsDto> GetParentsForStudents();
        StudentParentsDto GetParentsForStudent(int studentId);

        // CRUD without the C
        ParentDto GetParentById(int parentId);
        IEnumerable<ParentDto> GetAllParents();
        ParentDto UpdateParent(int parentId, ParentDto parent);
        ParentDto DeleteParent(int parentId);
    }
}