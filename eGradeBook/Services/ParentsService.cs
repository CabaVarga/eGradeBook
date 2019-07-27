using eGradeBook.Models;
using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace eGradeBook.Services
{
    public class ParentsService : IParentsService
    {
        private IUnitOfWork db;

        public ParentsService(IUnitOfWork db)
        {
            this.db = db;
        }

        public ParentChildrenDto AddChild(int parentId, int studentId)
        {
            ParentUser parent = db.ParentsRepository.Get(p => p.Id == parentId).FirstOrDefault();

            if (parent == null)
            {
                return null;
            }

            List<StudentUser> children = parent.Children.ToList();

            if (children.Where(c => c.Id == studentId).FirstOrDefault() != null)
            {
                return null;
            }

            StudentUser student = db.StudentsRepository.Get(s => s.Id == studentId).FirstOrDefault();

            if (student == null)
            {
                return null;
            }

            children.Add(student);

            parent.Children = children;

            db.Save();

            return new ParentChildrenDto()
            {
                Name = parent.FirstName + " " + parent.LastName,
                ParentId = parent.Id,
                Children = parent.Children.Select(c => new StudentDto()
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    ClassRoom = c.SchoolClass.Name,
                    ClassRoomId = c.SchoolClass.Id,
                    StudentId = c.Id
                }).ToList()
            };
        }

        public IEnumerable<StudentDto> GetAllChildren(int parentId)
        {
            ParentUser parent = db.ParentsRepository.Get(p => p.Id == parentId).FirstOrDefault();

            if (parent == null)
            {
                return null;
            }

            return parent.Children.Select(c => new StudentDto()
            {

                FirstName = c.FirstName,
                LastName = c.LastName,
                ClassRoom = c.SchoolClass.Name,
                ClassRoomId = c.SchoolClass.Id,
                StudentId = c.Id
            });
        }

        public IEnumerable<ParentChildrenDto> GetAllParentsWithTheirChildrent()
        {
            return db.ParentsRepository.Get()
                .Select(parent => new ParentChildrenDto()
                {
                    Name = parent.FirstName + " " + parent.LastName,
                    ParentId = parent.Id,
                    Children = parent.Children.Select(c => new StudentDto()
                    {
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        ClassRoom = c.SchoolClass.Name,
                        ClassRoomId = c.SchoolClass.Id,
                        StudentId = c.Id
                    }).ToList()
                });
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
                    Parents = s.Parents.Select(p => p.LastName + " " + p.FirstName).ToList()
                });
        }
    }
}