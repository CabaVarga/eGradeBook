using eGradeBook.Models;
using eGradeBook.Models.Dtos.StudentParents;
using eGradeBook.Repositories;
using eGradeBook.Services.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public class StudentParentsService : IStudentParentsService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IUnitOfWork db;

        public StudentParentsService(IUnitOfWork db)
        {
            this.db = db;
        }
                     
        public StudentParentDto CreateStudentParent(StudentParentDto studentParentDto)
        {
            var createdStudentParent = new StudentParent()
            {
                StudentId = studentParentDto.StudentId,
                ParentId = studentParentDto.ParentId
            };

            db.StudentParentsRepository.Insert(createdStudentParent);
            db.Save();

            return Converters.StudentParentsConverter.StudentParentToStudentParentDto(createdStudentParent);
        }

        public IEnumerable<StudentParentDto> GetAllStudentParents()
        {
            return db.StudentParentsRepository.Get()
                .Select(sp => Converters.StudentParentsConverter.StudentParentToStudentParentDto(sp));
        }

        public StudentParentDto GetStudentParentById(int studentParentId)
        {
            StudentParent studentParent = db.StudentParentsRepository.GetByID(studentParentId);

            return Converters.StudentParentsConverter.StudentParentToStudentParentDto(studentParent);
        }

        public IEnumerable<StudentParentDto> GetStudentParentsByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var studentParents = db.StudentParentsRepository.Get(
                g => (courseId != null ? g.Student.Enrollments.Any(e => e.Takings.Any(t => t.Program.Teaching.CourseId == courseId)) : true) &&
                    (teacherId != null ? g.Student.Enrollments.Any(e => e.Takings.Any(t => t.Program.Teaching.TeacherId == teacherId)) : true) &&
                    (classRoomId != null ? g.Student.Enrollments.Any(e => e.ClassRoomId == classRoomId) : true) &&
                    (studentId != null ? g.StudentId == studentId : true) &&
                    (parentId != null ? g.ParentId == parentId : true) &&
                    (schoolGrade != null ? g.Student.Enrollments.Any(e => e.ClassRoom.ClassGrade == schoolGrade) : true))
                    .Select(g => Converters.StudentParentsConverter.StudentParentToStudentParentDto(g));

            return studentParents;
        }

        public StudentParentDto DeleteStudentParentForReal(int studentParentId)
        {
            var sp = db.StudentParentsRepository.GetByID(studentParentId);
            if (sp == null)
            {
                return null;
            }
            var spDeleted = Converters.StudentParentsConverter.StudentParentToStudentParentDto(sp);

            db.StudentParentsRepository.Delete(sp);
            db.Save();

            return spDeleted;
        }
    }
}