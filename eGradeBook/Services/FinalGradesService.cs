using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.FinalGrades;
using eGradeBook.Repositories;

namespace eGradeBook.Services
{
    /// <summary>
    /// The service layer for working with final grades
    /// </summary>
    public class FinalGradesService : IFinalGradesService
    {
        /// <summary>
        /// We will be using the Unit of work as the repository layer orchestrator
        /// </summary>
        private IUnitOfWork db;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public FinalGradesService(IUnitOfWork db)
        {
            this.db = db;
        }

        /// <summary>
        /// Retrieve all final grades and convert them to Dtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FinalGradeDto> GetAllFinalGradesDto()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Retrieve all final grades for the given course
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IEnumerable<FinalGradeDto> GetAllFinalGradesForCourse(int courseId)
        {
            Course course = db.CoursesRepository.GetByID(courseId);

            if (course == null)
            {
                throw new Exception("Course not found");
            }

            return db.FinalGradesRepository.Get(fg => fg.Taking.Program.Course == course)
                .Select(fg => new FinalGradeDto()
                {
                    Student = fg.Taking.Student.FirstName + " " + fg.Taking.Student.LastName,
                    Subject = fg.Taking.Program.Course.Name,
                    SchoolGrade = fg.Taking.Program.SchoolClass.ClassGrade,
                    Semester = fg.SchoolTerm,
                    FinalGrade = fg.GradePoint
                });
        }

        /// <summary>
        /// Retrieve all final grades for the given student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public IEnumerable<FinalGradeDto> GetAllFinalGradesForStudent(int studentId)
        {
            StudentUser student = db.StudentsRepository.GetByID(studentId);

            if (student == null)
            {
                throw new Exception("Student not found");
            }

            // I had an exception when I have tried with fg.Taking.Studet == student
            // After I have added the id compare, it is working
            var finalGrades = db.FinalGradesRepository.Get(fg => fg.Taking.Student.Id == student.Id)
                .Select(fg => new FinalGradeDto()
                {
                    Student = fg.Taking.Student.FirstName + " " + fg.Taking.Student.LastName,
                    Subject = fg.Taking.Program.Course.Name,
                    SchoolGrade = fg.Taking.Program.SchoolClass.ClassGrade,
                    Semester = fg.SchoolTerm,
                    FinalGrade = fg.GradePoint
                });

            return finalGrades;
        }
    }
}