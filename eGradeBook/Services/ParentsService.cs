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

        public IEnumerable<ParentDto> GetAllParents()
        {
            return db.ParentsRepository.Get()
                .Select(p => Converters.ParentsConverter.ParentToParentDto(p));
        }

        /// <summary>
        /// Get a parent by Id
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public ParentDto GetParentById(int parentId)
        {
            var parent = db.ParentsRepository.GetByID(parentId);

            if (parent == null)
            {
                // Not exception but return NotFound from controller.
                return null;
            }

            return ParentsConverter.ParentToParentDto(parent);
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

        public IEnumerable<ParentDto> GetParentsByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var parents = db.ParentsRepository.Get(
                g => (courseId != null ? g.StudentParents.Any(sp => sp.Student.Enrollments.Any(e => e.Takings.Any(t => t.Program.Teaching.CourseId == courseId))) : true) &&
                    (teacherId != null ? g.StudentParents.Any(sp => sp.Student.Enrollments.Any(e => e.Takings.Any(t => t.Program.Teaching.TeacherId == teacherId))) : true) &&
                    (classRoomId != null ? g.StudentParents.Any(sp => sp.Student.Enrollments.Any(e => e.Takings.Any(t => t.Program.ClassRoomId == classRoomId))) : true) &&
                    (studentId != null ? g.StudentParents.Any(sp => sp.StudentId == studentId) : true) &&
                    (parentId != null ? g.Id == parentId : true) &&
                    (schoolGrade != null ? g.StudentParents.Any(sp => sp.Student.Enrollments.Any(e => e.ClassRoom.ClassGrade == schoolGrade)) : true))
                    .Select(g => Converters.ParentsConverter.ParentToParentDto(g));

            return parents;
        }
    }
}