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
        IEnumerable<TeacherDto> GetAllTeachersDtos();
        TeacherDto GetTeacherByIdDto(int id);

        void AssignCourseToTeacher(TeachingAssignmentDto assignment);
    }
}