using eGradeBook.Models;
using eGradeBook.Models.Dtos.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface ITeachersService
    {
        // These will must go
        IEnumerable<TeacherDto> GetAllTeachersDtos();
        TeacherDto GetTeacherByIdDto(int id);

        void AssignCourseToTeacher(TeachingAssignmentDto assignment);

        // CRUD without the C
        TeacherDto GetTeacherById(int teacherId);
        IEnumerable<TeacherDto> GetAllTeachers();
        TeacherDto UpdateTeacher(int teacherId, TeacherDto teacher);
        TeacherDto DeleteTeacher(int teacherId);
    }
}