using eGradeBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface IStudentsService
    {
        IEnumerable<StudentUser> GetStudentsByNameStartingWith(string start);

        // Course GPA Dto GetGpaForStudentCourse(int studentId, int courseId);
        // Student Courses GPA (by course and total) GetGpaForStudent(int studentId)
    }
}