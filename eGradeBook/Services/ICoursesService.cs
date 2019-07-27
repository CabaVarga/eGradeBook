using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface ICoursesService
    {
        // FULL MODEL CRUD
        IEnumerable<Course> GetAllCourses();

        Course GetCourseById(int id);

        Course CreateCourse(Course course);

        Course UpdateCourse(int id, Course course);

        Course DeleteCourse(int id);

        // Admin Course Management by Dtos
        CourseDto CreateCourse(CourseDto course);
        CourseDto UpdateCourse(CourseDto course);
        CourseDto DeleteCourse(CourseDto course);

        // Assign and remove teacher
        void AssignTeacherToCourse(int teacherId, int courseId);
        void RemoveTeacherFromCourse(int teacherId, int courseId);
    }
}