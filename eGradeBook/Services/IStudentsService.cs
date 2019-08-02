﻿using eGradeBook.Models;
using eGradeBook.Models.Dtos.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Interface for students related tasks
    /// </summary>
    public interface IStudentsService
    {
        /// <summary>
        /// Retrieve students by their name starting with
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        IEnumerable<StudentUser> GetStudentsByNameStartingWith(string start);

        // This will have to be gone
        /// <summary>
        /// Retrieve all students
        /// </summary>
        /// <returns></returns>
        IEnumerable<StudentUser> GetAllStudents();

        // Course GPA Dto GetGpaForStudentCourse(int studentId, int courseId);
        // Student Courses GPA (by course and total) GetGpaForStudent(int studentId)

        // CRUD without the C
        /// <summary>
        /// Get a student by Id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentDto GetStudentById(int studentId);

        // IEnumerable<StudentDto> GetAllStudents();

        /// <summary>
        /// Update a student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="student"></param>
        /// <returns></returns>
        StudentDto UpdateStudent(int studentId, StudentDto student);

        /// <summary>
        /// Delete a student
        /// TODO what should I do with these entries? If I'm using Identity for changes?
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        StudentDto DeleteStudent(int studentId);

        /// <summary>
        /// Retrieve all students
        /// </summary>
        /// <returns></returns>
        IEnumerable<StudentDto> GetAllStudentsDto()
    }
}