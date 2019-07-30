using eGradeBook.Models;
using eGradeBook.Models.Dtos.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface IStudentsService
    {
        IEnumerable<StudentUser> GetStudentsByNameStartingWith(string start);

        // This will have to be gone
        IEnumerable<StudentUser> GetAllStudents();

        // Course GPA Dto GetGpaForStudentCourse(int studentId, int courseId);
        // Student Courses GPA (by course and total) GetGpaForStudent(int studentId)

        // CRUD without the C
        StudentDto GetStudentById(int studentId);
        // IEnumerable<StudentDto> GetAllStudents();
        StudentDto UpdateStudent(int studentId, StudentDto student);
        StudentDto DeleteStudent(int studentId);
    }
}