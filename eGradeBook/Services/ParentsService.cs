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
                    ClassRoom = c.Student.SchoolClass.Name,
                    ClassRoomId = c.Student.SchoolClass.Id,
                    StudentId = c.Student.Id
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

            return parent.StudentParents.Select(c => new StudentDto()
            {

                FirstName = c.Student.FirstName,
                LastName = c.Student.LastName,
                ClassRoom = c.Student.SchoolClass.Name,
                ClassRoomId = c.Student.SchoolClass.Id,
                StudentId = c.Student.Id
            });
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
                        ClassRoom = c.Student.SchoolClass.Name,
                        ClassRoomId = c.Student.SchoolClass.Id,
                        StudentId = c.Student.Id
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
                    Parents = s.StudentParents.Select(p => p.Parent.LastName + " " + p.Parent.FirstName).ToList()
                });
        }
    }
}