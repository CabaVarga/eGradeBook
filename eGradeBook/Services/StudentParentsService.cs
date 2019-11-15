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
        private Lazy<IStudentsService> studentsService;
        private Lazy<IParentsService> parentsService;

        public StudentParentsService(
            IUnitOfWork db,
            Lazy<IStudentsService> studentsService,
            Lazy<IParentsService> parentsService)
        {
            this.db = db;
            this.studentsService = studentsService;
            this.parentsService = parentsService;
        }

        public StudentParent CreateStudentParent(StudentParentDto studentParentDto)
        {
            return CreateStudentParent(studentParentDto.StudentId, studentParentDto.ParentId);
        }

        public StudentParent CreateStudentParent(int studentId, int parentId)
        {
            StudentUser student = studentsService.Value.GetStudentById(studentId);

            ParentUser parent = parentsService.Value.GetParentById(parentId);

            StudentParent newStudentParent = new StudentParent()
            {
                Student = student,
                Parent = parent
            };

            db.StudentParentsRepository.Insert(newStudentParent);
            db.Save();

            return newStudentParent;
        }

        public StudentParentDto CreateStudentParentDto(StudentParentDto studentParentDto)
        {
            return CreateStudentParentDto(studentParentDto.StudentId, studentParentDto.ParentId);
        }

        public StudentParentDto CreateStudentParentDto(int studentId, int parentId)
        {
            StudentParent createdStudentParent = CreateStudentParent(studentId, parentId);

            return Converters.StudentParentsConverter.StudentParentToStudentParentDto(createdStudentParent);
        }

        public StudentParent DeleteStudentParent(StudentParentDto studentParentDto)
        {
            return DeleteStudentParent(studentParentDto.StudentId, studentParentDto.ParentId);
        }

        public StudentParent DeleteStudentParent(int studentId, int parentId)
        {
            StudentParent studentParent = GetStudentParent(studentId, parentId);

            db.StudentParentsRepository.Delete(studentParent);
            db.Save();

            return studentParent;
        }

        public StudentParent DeleteStudentParent(int studentParentId)
        {
            StudentParent studentParent = GetStudentParent(studentParentId);



            db.StudentParentsRepository.Delete(studentParent);
            db.Save();

            return studentParent;
        }

        public StudentParentDto DeleteStudentParentDto(StudentParentDto studentParentDto)
        {
            return DeleteStudentParentDto(studentParentDto.StudentId, studentParentDto.ParentId);
        }

        public StudentParentDto DeleteStudentParentDto(int studentId, int parentId)
        {
            StudentParent deletedStudentParent = DeleteStudentParent(studentId, parentId);

            return Converters.StudentParentsConverter.StudentParentToStudentParentDto(deletedStudentParent);
        }

        public StudentParentDto DeleteStudentParentDto(int studentParentId)
        {
            StudentParent deletedStudentParent = DeleteStudentParent(studentParentId);

            return Converters.StudentParentsConverter.StudentParentToStudentParentDto(deletedStudentParent);
        }

        public IEnumerable<StudentParent> GetAllStudentParents()
        {
            return db.StudentParentsRepository.Get();
        }

        public IEnumerable<StudentParentDto> GetAllStudentParentsDto()
        {
            return GetAllStudentParents()
                .Select(sp => Converters.StudentParentsConverter.StudentParentToStudentParentDto(sp));
        }

        public StudentParent GetStudentParent(StudentParentDto studentParentDto)
        {
            return GetStudentParent(studentParentDto.StudentId, studentParentDto.ParentId);
        }

        public StudentParent GetStudentParent(int studentId, int parentId)
        {
            StudentUser student = studentsService.Value.GetStudentById(studentId);

            ParentUser parent = parentsService.Value.GetParentById(parentId);

            var studentParent = db.StudentParentsRepository.Get(sp => sp.StudentId == studentId && sp.ParentId == parentId).FirstOrDefault();

            if (studentParent == null)
            {
                logger.Info("Student {@studentId} is not related to Parent {@parentId}", studentId, parentId);
                var ex = new StudentParentNotFoundException(string.Format("Student {0} is not related to Parent {1}", studentId, parentId));
                ex.Data.Add("studentId", studentId);
                ex.Data.Add("parentId", parentId);
                throw ex;
            }

            return studentParent;
        }

        public StudentParent GetStudentParent(int studentParentId)
        {
            var studentParent = db.StudentParentsRepository.GetByID(studentParentId);

            if (studentParent == null)
            {
                logger.Info("StudentParent {@studentParentId} not found", studentParentId);
                var ex = new StudentParentNotFoundException(string.Format("StudentParent {0} not found", studentParentId));
                ex.Data.Add("studentParentId", studentParentId);
                throw ex;
            }

            return studentParent;
        }

        public StudentParentDto GetStudentParentDto(StudentParentDto studentParentDto)
        {
            var studentParent = GetStudentParent(studentParentDto);

            return Converters.StudentParentsConverter.StudentParentToStudentParentDto(studentParent);
        }

        public StudentParentDto GetStudentParentDto(int studentId, int parentId)
        {
            var studentParent = GetStudentParent(studentId, parentId);

            return Converters.StudentParentsConverter.StudentParentToStudentParentDto(studentParent);
        }

        public StudentParentDto GetStudentParentDto(int studentParentId)
        {
            StudentParent studentParent = GetStudentParent(studentParentId);

            return Converters.StudentParentsConverter.StudentParentToStudentParentDto(studentParent);
        }

        public IEnumerable<StudentParentDto> GetStudentParentsByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var studentParents = db.StudentParentsRepository.Get(
                g => (courseId != null ? g.Student.Takings.Any(t => t.Program.Teaching.CourseId == courseId) : true) &&
                    (teacherId != null ? g.Student.Takings.Any(t => t.Program.Teaching.TeacherId == teacherId) : true) &&
                    (classRoomId != null ? g.Student.ClassRoomId == classRoomId : true) &&
                    (studentId != null ? g.StudentId == studentId : true) &&
                    (parentId != null ? g.ParentId == parentId : true) &&
                    (schoolGrade != null ? g.Student.ClassRoom.ClassGrade == schoolGrade : true))
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