using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Repositories;
using eGradeBook.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace eGradeBook.Services
{
    public class GradesService : IGradesService
    {
        private IUnitOfWork db;

        public GradesService(IUnitOfWork db)
        {
            this.db = db;
        }

        public GradeDto CreateGrade(int teacherId, int studentId, int subjectId, int gradePoint, string notes = null)
        {
            // Teacher Id must be same as the teacher that is accessing the system...
            // If it's not then he/she should not even get here!
            // Should that be done in the controller by accessing the authrepo? probably...


            // check if teacherId is valid ---> I need to check if a Teacher repo is possible... let's find out...
            // it will raise an exception if the id is not of a teacher!
            // what now? Typechecks...
            var teacherMaybe = db.GradeBookUsersRepository.GetByID(teacherId);

            // first check for null

            if (teacherMaybe == null)
            {
                throw new InvalidUserIdException($"There is no registered user in the system for the provided Id: {teacherId}");
            }

            // then check for teacher
            // many ways....
            bool itsateacher = teacherMaybe is TeacherUser;
            bool itsastudent = teacherMaybe is StudentUser;
            bool itsanadmin = teacherMaybe is AdminUser;
            bool itsaparent = teacherMaybe is ParentUser;

            if (!itsateacher)
            {
                throw new Exception("Unauthorized");
            }

            // Same thing, must check for Type again
            //var teacher = db.TeachersRepository.GetByID(teacherId);

            //if (teacher == null)
            //{
            //    throw new Exception("Teacher Id is invalid");
            //}

            // students repo...
            var student = db.StudentsRepository.GetByID(studentId);

            if (student == null)
            {
                throw new Exception("Student Id is invalid");
            }

            // must check for student!
            // returns all users unfortunately, except when using typeof stuff..

            // check subject --- repooo...
            // its called course...
            var subject = db.CoursesRepository.GetByID(subjectId);

            if (subject == null)
            {
                throw new Exception("Subject Id is invalid");
            }

            // check if teacher teaches the subject
            var assignment = db.TeachingAssignmentsRepository.Get(filter: a => a.SubjectId == subjectId && a.TeacherId == teacherId).FirstOrDefault();

            if (assignment == null)
            {
                throw new Exception("Teacher is not teaching the subject");
            }

            // we need to get to the Takings
            // we need 1. assignment -> we have that
            // we need 2. classRoom (or schoolClass)
            // we need 3. program
            // based on the above 3 we can finally get the taking..

            var schoolClass = student.SchoolClass;

            var program = db.ProgramsRepository.Get(p => p.SchoolClass == schoolClass && p.Teaching == assignment && p.Course == subject).FirstOrDefault();

            // easy way, if null then bad for you. no helpful exception handling though...
            var taking = db.TakingsRepository.Get(t => t.Program.CourseId == subjectId && t.Program.Teaching.TeacherId == teacherId
                && t.StudentId == studentId).FirstOrDefault();

            // School Term (or semester) check
            // Final Grade check (if there is a final grade for a given semester, you cant assign a new grade
            // also: without final grade for first semester cant assign a grade into the new semester...

            Grade grade = new Grade()
            {
                GradePoint = gradePoint,
                Advancement = taking,
                Assigned = DateTime.UtcNow,
                Notes = notes,
                SchoolTerm = 1
            };

            db.GradesRepository.Insert(grade);
            db.Save();

            return new GradeDto()
            {
                GradePoint = gradePoint,
                Subject = subject.Name,
                StudentName = student.FirstName + " " + student.LastName,
                TeacherName = teacherMaybe.FirstName + " " + teacherMaybe.LastName
            };
        }

        public IEnumerable<GradeDto> GetAllGrades()
        {
            // This is the most basic implementation, with every grade included, no ordering, no grouping..
            var grades = db.GradesRepository.Get()
                .Select(g =>
                new GradeDto()
                {
                    GradePoint = g.GradePoint,
                    Subject = g.Advancement.Program.Teaching.Course.Name,
                    StudentName = g.Advancement.Student.FirstName + " " + g.Advancement.Student.LastName,
                    TeacherName = g.Advancement.Program.Teaching.Teacher.FirstName + " " + g.Advancement.Program.Teaching.Teacher.LastName
                });

            return grades;
        }

        public IEnumerable<GradeDto> GetAllGradesForTeacher(int teacherId)
        {
            var grades = db.GradesRepository.Get(
                filter: g => g.Advancement.Program.Teaching.TeacherId == teacherId)
                .Select(g =>
                new GradeDto()
                {
                    GradePoint = g.GradePoint,
                    Subject = g.Advancement.Program.Teaching.Course.Name,
                    StudentName = g.Advancement.Student.FirstName + " " + g.Advancement.Student.LastName,
                    TeacherName = g.Advancement.Program.Teaching.Teacher.FirstName + " " + g.Advancement.Program.Teaching.Teacher.LastName
                });

            return grades;
        }

        public IEnumerable<GradeDto> GetAllGradesForStudent(int studentId)
        {
            var grades = db.GradesRepository.Get(
                filter: g => g.Advancement.StudentId == studentId)
                .Select(g =>
                new GradeDto()
                {
                    GradePoint = g.GradePoint,
                    Subject = g.Advancement.Program.Teaching.Course.Name,
                    StudentName = g.Advancement.Student.FirstName + " " + g.Advancement.Student.LastName,
                    TeacherName = g.Advancement.Program.Teaching.Teacher.FirstName + " " + g.Advancement.Program.Teaching.Teacher.LastName
                });

            return grades;
        }

        public IEnumerable<GradeDto> GetAllGradesForParent(int parentId)
        {
            var children = db.ParentsRepository.GetByID(parentId).Children;
            var childrenIds = new List<int>();

            foreach (var c in children)
            {
                childrenIds.Add(c.Id);
            }

            // Hmm... i'm not quite it will work...
            var grades = db.GradesRepository.Get(g => g.Advancement.Student.Parents.Any(p => p.Id == parentId))
                .Select(g =>
                new GradeDto()
                {
                    GradePoint = g.GradePoint,
                    Subject = g.Advancement.Program.Teaching.Course.Name,
                    StudentName = g.Advancement.Student.FirstName + " " + g.Advancement.Student.LastName,
                    TeacherName = g.Advancement.Program.Teaching.Teacher.FirstName + " " + g.Advancement.Program.Teaching.Teacher.LastName
                });

            return grades;
        }

        public IEnumerable<GradeDto> GetGradesByCourses()
        {
            var grades = db.GradesRepository.Get().Select(g => new GradeDto()
            {
                GradePoint = g.GradePoint,
                StudentName = g.Advancement.Student.FirstName + " " + g.Advancement.Student.LastName,
                Subject = g.Advancement.Program.Course.Name,
                TeacherName = g.Advancement.Program.Teaching.Teacher.FirstName + " " + g.Advancement.Program.Teaching.Teacher.LastName
            });


            return grades;
        }

        public IEnumerable<GradeDto> GetGradesByParameters(int? studentId, int? gradeId, int? teacherId, int? courseId, int? semesterId, int? classId)
        {
            
            Func<Grade, bool> filter =
                g => studentId != null ? g.Advancement.StudentId == studentId : true &&
                    gradeId != null ? g.Advancement.Program.SchoolClass.ClassGrade == gradeId : true &&
                    teacherId != null ? g.Advancement.Program.Teaching.TeacherId == teacherId : true &&
                    courseId != null ? g.Advancement.Program.CourseId == courseId : true &&
                    semesterId != null ? g.SchoolTerm == semesterId : true &&
                    classId != null ? g.Advancement.Program.SchoolClassId == classId : true;

            return db.GradesRepository.Get(
                filter:
                g => studentId != null ? g.Advancement.StudentId == studentId : true &&
                    gradeId != null ? g.Advancement.Program.SchoolClass.ClassGrade == gradeId : true &&
                    teacherId != null ? g.Advancement.Program.Teaching.TeacherId == teacherId : true &&
                    courseId != null ? g.Advancement.Program.CourseId == courseId : true &&
                    semesterId != null ? g.SchoolTerm == semesterId : true &&
                    classId != null ? g.Advancement.Program.SchoolClassId == classId : true)
                    .Select(g => new GradeDto()
                    {
                        StudentName = g.Advancement.Student.FirstName + " " + g.Advancement.Student.LastName,
                        Subject = g.Advancement.Program.Course.Name,
                        GradePoint = g.GradePoint,
                        TeacherName = g.Advancement.Program.Teaching.Teacher.FirstName + " " + g.Advancement.Program.Teaching.Teacher.LastName
                    });

        }
    }
}