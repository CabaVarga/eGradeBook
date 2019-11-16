using eGradeBook.Models;
using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
using eGradeBook.Services.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with teacher entities and related tasks
    /// </summary>
    public class TeachersService : ITeachersService
    {
        private IUnitOfWork db;
        private ILogger logger;
        private Lazy<ITeachingsService> teachingsService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="teachingsService"></param>
        public TeachersService(IUnitOfWork db, 
            Lazy<ITeachingsService> teachingsService, 
            ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }



        /// <summary>
        /// Delete a teacher user from the system
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task<TeacherDto> DeleteTeacher(int teacherId)
        {
            logger.Info("Service received request for deleting a teacher {teacherId}", teacherId);

            TeacherUser deletedTeacher = db.TeachersRepository.Get(t => t.Id == teacherId).FirstOrDefault();

            if (deletedTeacher == null)
            {
                return null;
            }

            var result = await db.AuthRepository.DeleteUser(teacherId);

            if (!result.Succeeded)
            {
                logger.Error("User removal failed {errors}", result.Errors);
                //return null;
                throw new ConflictException("Delete teacher failed in auth repo");
            }

            return TeachersConverter.TeacherToTeacherDto(deletedTeacher);
        }



        /// <summary>
        /// Get all teachers as IEnumerable of TeacherDto
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeacherDto> GetAllTeachers()
        {
            logger.Info("Service received request for retrieving all teachers");

            return db.TeachersRepository.Get()
                .OfType<TeacherUser>()
                .Select(t => Converters.TeachersConverter.TeacherToTeacherDto(t)).ToList();
        }

        /// <summary>
        /// Almost same as GetTeacherById
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TeacherDto GetTeacherById(int id)
        {
            logger.Info("Service received request for retrieving teacher by Id {teacherId}", id);

            var t = db.TeachersRepository.GetByID(id);


            var dto = Converters.TeachersConverter.TeacherToTeacherDto(t);

            return dto;

        }

        /// <summary>
        /// Update teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="teacher"></param>
        /// <returns></returns>
        public TeacherDto UpdateTeacher(int teacherId, TeacherDto teacher)
        {
            logger.Info("Service received request for updating teacher {teacherId} with data {@teacher}", teacherId, teacher);

            throw new NotImplementedException();
        }







        public IEnumerable<TeacherDto> GetTeachersByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var teachers = db.TeachersRepository.Get(
                g => (courseId != null ? g.Teachings.Any(t => t.CourseId == courseId) : true) &&
                    (teacherId != null ? g.Id == teacherId : true) &&
                    (classRoomId != null ? g.Teachings.Any(t => t.Programs.Any(p => p.ClassRoomId == classRoomId)) : true) &&
                    (studentId != null ? g.Teachings.Any(t => t.Programs.Any(p => p.Takings.Any(tk => tk.Enrollment.StudentId == studentId))) : true) &&
                    (parentId != null ? g.Teachings.Any(t => t.Programs.Any(p => p.Takings.Any(tk => tk.Enrollment.Student.StudentParents.Any(sp => sp.ParentId == parentId)))) : true) &&
                    (schoolGrade != null ? g.Teachings.Any(t => t.Programs.Any(p => p.ClassRoom.ClassGrade == schoolGrade)) : true))
                    .Select(g => Converters.TeachersConverter.TeacherToTeacherDto(g));

            return teachers;
        }
    }
}