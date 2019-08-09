using eGradeBook.Models;
using eGradeBook.Models.Dtos.StudentParents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services.Converters
{
    public static class StudentParentsConverter
    {
        public static StudentParentDto StudentParentToStudentParentDto(StudentParent studentParent)
        {
            return new StudentParentDto()
            {
                StudentParentId = studentParent.Id,
                StudentId = studentParent.Student.Id,
                ParentId = studentParent.Parent.Id,
                StudentName = studentParent.Student.UserName,
                ParentName = studentParent.Parent.UserName
            };
        }
    }
}