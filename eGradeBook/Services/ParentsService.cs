using eGradeBook.Models;
using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Repositories;
using NLog;
using System.Collections.Generic;
using System.Linq;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with parents and related tasks
    /// </summary>
    public class ParentsService : IParentsService
    {
        private IUnitOfWork db;
        private ILogger logger;

        public ParentsService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public ParentChildrenDto AddChild(int parentId, int studentId)
        {
            ParentUser parent = db.ParentsRepository.Get(p => p.Id == parentId).FirstOrDefault();

            if (parent == null)
            {
                return null;
            }

            StudentUser student = db.StudentsRepository.Get(s => s.Id == studentId).FirstOrDefault();

            if (student == null)
            {
                return null;
            }

            StudentParent sp = new StudentParent()
            {
                Student = student,
                Parent = parent
            };

            db.StudentParentsRepository.Insert(sp);            
            db.Save();

            return new ParentChildrenDto()
            {
                Name = parent.FirstName + " " + parent.LastName,
                ParentId = parent.Id,
                Children = parent.StudentParents.Select(c => new StudentDto()
                {
                    FirstName = c.Student.FirstName,
                    LastName = c.Student.LastName,
                    ClassRoom = c.Student.ClassRoom.Name,
                    ClassRoomId = c.Student.ClassRoom.Id,
                    StudentId = c.Student.Id
                }).ToList()
            };
        }

        public ParentDto DeleteParent(int parentId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<StudentDto> GetAllChildren(int parentId)
        {
            ParentUser parent = db.ParentsRepository.Get(p => p.Id == parentId).FirstOrDefault();

            if (parent == null)
            {
                return null;
            }

            return parent.StudentParents.Select(c => new StudentDto()
            {

                FirstName = c.Student.FirstName,
                LastName = c.Student.LastName,
                ClassRoom = c.Student.ClassRoom.Name,
                ClassRoomId = c.Student.ClassRoom.Id,
                StudentId = c.Student.Id
            });
        }

        public IEnumerable<ParentDto> GetAllParents()
        {
            return db.ParentsRepository.Get()
                .Select(p => Converters.ParentsConverter.ParentToParentDto(p));
        }

        public IEnumerable<ParentChildrenDto> GetAllParentsWithTheirChildrent()
        {
            return db.ParentsRepository.Get()
                .Select(parent => new ParentChildrenDto()
                {
                    Name = parent.FirstName + " " + parent.LastName,
                    ParentId = parent.Id,
                    Children = parent.StudentParents.Select(c => new StudentDto()
                    {
                        FirstName = c.Student.FirstName,
                        LastName = c.Student.LastName,
                        ClassRoom = c.Student.ClassRoom.Name,
                        ClassRoomId = c.Student.ClassRoom.Id,
                        StudentId = c.Student.Id
                    }).ToList()
                });
        }

        public ParentDto GetParentById(int parentId)
        {
            throw new System.NotImplementedException();
        }

        public StudentParentsDto GetParentsForStudent(int studentId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<StudentParentsDto> GetParentsForStudents()
        {
            return db.StudentsRepository.Get()
                .Select(s => new StudentParentsDto()
                {
                    Name = s.FirstName + " " + s.LastName,
                    Parents = s.StudentParents.Select(p => p.Parent.LastName + " " + p.Parent.FirstName).ToList()
                });
        }

        public ParentDto UpdateParent(int parentId, ParentDto parent)
        {
            throw new System.NotImplementedException();
        }
    }
}