using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.FinalGrades;
using eGradeBook.Repositories;
using NLog;

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
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public FinalGradesService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
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
                .Select(fg => Converters.FinalGradesConverter.FinalGradeToFinalGradeDto(fg));
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
                .Select(fg => Converters.FinalGradesConverter.FinalGradeToFinalGradeDto(fg));

            return finalGrades;
        }

        public FinalGrade GetFinalGradeById(int finalGradeId)
        {
            logger.Info("Get final grade by Id {@finalGradeId}", finalGradeId);

            return db.FinalGradesRepository.GetByID(finalGradeId);
        }

        public FinalGradeDto GetFinalGradeDtoById(int finalGradeId)
        {
            logger.Info("Get final grade dto by Id {@finalGradeId}", finalGradeId);

            FinalGrade finalGrade = GetFinalGradeById(finalGradeId);

            return Converters.FinalGradesConverter.FinalGradeToFinalGradeDto(finalGrade);
        }
    }
}