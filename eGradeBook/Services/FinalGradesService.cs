using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.FinalGrades;
using eGradeBook.Repositories;

namespace eGradeBook.Services
{
    public class FinalGradesService : IFinalGradesService
    {
        private IUnitOfWork db;

        public FinalGradesService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<FinalGradeDto> GetAllFinalGradesDto()
        {
            throw new NotImplementedException();
        }

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