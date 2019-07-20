using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Repositories;

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

            // then check for teacher
            // many ways....
            bool itsateacher = teacherMaybe is TeacherUser;
            bool itsastudent = teacherMaybe is StudentUser;
            bool itsanadmin = teacherMaybe is AdminUser;

            bool itsaparent = teacherMaybe is ParentUser;

            var teacher = db.TeachersRepository.GetByID(teacherId);

            if (teacher == null)
            {
                throw new Exception("Teacher Id is invalid");
            }

            // students repo...
            var student = db.StudentsRepository.GetByID(studentId);

            if (student == null)
            {
                throw new Exception("Student Id is invalid");
            }

            // check subject --- repooo...
            var subject = db.SubjectsRepository.GetByID(subjectId);

            if (subject == null)
            {
                throw new Exception("Subject Id is invalid");
            }

            // check if teacher teaches the subject
            var assignments = db.TeachingAssignmentsRepository.Get(filter: a => a.SubjectId == subjectId && a.TeacherId == teacherId).FirstOrDefault();

            if (assignments == null)
            {
                throw new Exception("Teacher is not teaching the subject");
            }

            // check if student is in the classroom where teacher teaches the subject
            if (student.ClassRoomId != assignments.ClassRoomId)
            {
                throw new Exception("Student does not taking the subject with the teacher");
            }

            // Ok, every Id is valid, teacher teaches the subject to the student.
            Grade grade = new Grade()
            {
                GradePoint = gradePoint,
                Student = student,
                Teacher = teacher,
                Subject = subject,
                Assigned = DateTime.UtcNow,
                Notes = notes
            };

            db.GradesRepository.Insert(grade);
            db.Save();

            return new GradeDto()
            {
                GradePoint = gradePoint,
                Subject = subject.Name,
                StudentName = student.FirstName + " " + student.LastName,
                TeacherName = teacher.FirstName + " " + teacher.LastName
            };
        }

        public IEnumerable<GradeDto> GetAllGrades()
        {
            // This is the most basic implementation, with every grade included, no ordering, no grouping..
            var grades = db.GradesRepository.Get(includeProperties: "Subject,Student,Teacher")
                .Select(g =>
                new GradeDto()
                {
                    GradePoint = g.GradePoint,
                    Subject = g.Subject.Name,
                    StudentName = g.Student.FirstName + " " + g.Student.LastName,
                    TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName
                });

            return grades;            
        }

        public IEnumerable<GradeDto> GetAllGradesForTeacher(int teacherId)
        {
            var grades = db.GradesRepository.Get(
                filter: g => g.TeacherId == teacherId,
                includeProperties: "Subject,Student,Teacher")
                .Select(g =>
                new GradeDto()
                {
                    GradePoint = g.GradePoint,
                    Subject = g.Subject.Name,
                    StudentName = g.Student.FirstName + " " + g.Student.LastName,
                    TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName
                });

            return grades;
        }

        public IEnumerable<GradeDto> GetAllGradesForStudent(int studentId)
        {
            var grades = db.GradesRepository.Get(
                filter: g => g.StudentId == studentId,
                includeProperties: "Subject,Student,Teacher")
                .Select(g =>
                new GradeDto()
                {
                    GradePoint = g.GradePoint,
                    Subject = g.Subject.Name,
                    StudentName = g.Student.FirstName + " " + g.Student.LastName,
                    TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName
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
            var grades = db.GradesRepository.Get(
                filter: g => childrenIds.Any(el => el == g.StudentId),
                includeProperties: "Subject,Student,Teacher")
                .Select(g =>
                new GradeDto()
                {
                    GradePoint = g.GradePoint,
                    Subject = g.Subject.Name,
                    StudentName = g.Student.FirstName + " " + g.Student.LastName,
                    TeacherName = g.Teacher.FirstName + " " + g.Teacher.LastName
                });

            return grades;
        }
    }
}