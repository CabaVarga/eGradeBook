using eGradeBook.Models;
using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
using eGradeBook.Services.Exceptions;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with parents and related tasks
    /// </summary>
    public class ParentsService : IParentsService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public ParentsService(IUnitOfWork db)
        {
            this.db = db;
        }

        /// <summary>
        /// Add a student to the parent as a child
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public ParentChildrenDto AddChild(int parentId, int studentId)
        {
            logger.Info("Make {@userId} parent of {@userId}", parentId, studentId);

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

            // What even to return???

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

        /// <summary>
        /// Get a parent by Id
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ParentDto GetParentByIdDto(int parentId)
        {
            var parent = GetParentById(parentId);

            if (parent == null)
            {
                // Not exception but return NotFound from controller.
                return null;
            }

            return ParentsConverter.ParentToParentDto(parent);
        }

        public ParentUser GetParentById(int parentId)
        {
            ParentUser parent = db.ParentsRepository.Get(p => p.Id == parentId).FirstOrDefault();

            if (parent == null)
            {
                // Not exception but return NotFound from controller.
                return null;
            }

            return parent;
        }

        /// <summary>
        /// Probably should by in Student? I cannot decide...
        /// Most probably not even an independent method... or API
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public StudentParentsDto GetParentsForStudent(int studentId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// This was only a test method
        /// TODO delete
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StudentParentsDto> GetParentsForStudents()
        {
            return db.StudentsRepository.Get()
                .Select(s => new StudentParentsDto()
                {
                    Name = s.FirstName + " " + s.LastName,
                    Parents = s.StudentParents.Select(p => p.Parent.LastName + " " + p.Parent.FirstName).ToList()
                });
        }

        public ParentReportDto GetParentReport(int parentId)
        {
            ParentUser parent = GetParentById(parentId);

            return Converters.ParentsConverter.ParentToParentReportDto(parent);
        }

        public async Task<ParentDto> DeleteParent(int parentId)
        {
            logger.Info("Service received request for deleting a parent {parentId}", parentId);

            var deletedParent = db.ParentsRepository.Get(p => p.Id == parentId).FirstOrDefault();

            if (deletedParent == null)
            {
                return null;
            }

            var result = await db.AuthRepository.DeleteUser(parentId);

            if (!result.Succeeded)
            {
                logger.Error("Parent removal failed {errors}", result.Errors);
                //return null;
                throw new ConflictException("Delete parent failed in auth repo");
            }

            return Converters.ParentsConverter.ParentToParentDto(deletedParent);
        }
    }
}