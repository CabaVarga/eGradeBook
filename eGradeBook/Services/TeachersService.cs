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
        /// Assign a course to a teacher
        /// </summary>
        /// <param name="assignment"></param>
        public void AssignCourseToTeacher(TeachingAssignmentDto assignment)
        {
            logger.Info("Service received request for assigning a course to a teacher {@teachingAssignment}", assignment);
            // but again ... course --> teaching. teaching --> course...
            // Or another service that will reference everything but is not referenced by anything?
            // SHAT!!!
            // teachingsService.AssignTeacherToCourse(assignment.SubjectId, assignment.TeacherId);
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
        /// Retrieve all teachers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeacherDto> GetAllTeachers()
        {
            logger.Info("Service received request for retrieving all teachers");

            return db.TeachersRepository.Get()
                // maybe won't work?
                // also, without include it will take a number of roundtrips to the database...
                .Select(t => TeachersConverter.TeacherToTeacherDto(t));
        }

        /// <summary>
        /// Get all teachers as IEnumerable of TeacherDto
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TeacherDto> GetAllTeachersDtos()
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
        public TeacherDto GetTeacherByIdDto(int id)
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

        public TeacherExtendedDto GetExtendedDataForTeacher(int teacherId)
        {
            TeacherUser teacher = db.TeachersRepository.Get(t => t.Id == teacherId).FirstOrDefault();

            if (teacher == null)
            {
                return null;
            }

            TeacherExtendedDto teacherData = new TeacherExtendedDto()
            {
                TeacherId = teacher.Id,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                ClassRooms = teacher.Teachings
                .SelectMany(t => t.Programs).GroupBy(p => p.ClassRoom)
                .Select(g => new TeacherExtendedDto.ClassRoomCoursesDto()
                {
                    ClassRoomId = g.Key.Id,
                    ClassRoomName = g.Key.Name,
                    Grade = g.Key.ClassGrade,
                    Courses = g.Select(prog => new TeacherExtendedDto.ClassRoomCoursesDto.InnerCourseDto()
                    {
                        CourseId = prog.Course.Id,
                        CourseName = prog.Course.Name,
                        WeeklyHours = prog.WeeklyHours
                    }).ToList()
                }).ToList(),
                // No groupings or selectmanys?? and still working...
                Courses = teacher.Teachings
                .Select(g => new TeacherExtendedDto.CourseClassRoomDto()
                {
                CourseId = g.Course.Id,
                CourseName = g.Course.Name,
                ClassRooms = g.Programs.Select(p => new TeacherExtendedDto.CourseClassRoomDto.InnerClassRoomDto()
                {
                ClassRoomId = p.ClassRoom.Id,
                ClassRoomName = p.ClassRoom.Name,
                Grade = p.ClassRoom.ClassGrade,
                WeeklyHours = p.WeeklyHours
                }).ToList()
                }).ToList()
            };

            return teacherData;
        }

        public object GetClassRoomsForTeacher(int teacherId)
        {
            return db.TeachersRepository.Get(t => t.Id == teacherId)
    .Select(t => new
    {
        TeacherId = t.Id,
        FName = t.FirstName,
        LName = t.LastName,
        ClassRooms = t.Teachings.GroupBy(ts => ts.Programs).Select(p => new
        {
            ClassRoom = p.Key.Select(k => new { k.ClassRoom.Id, k.ClassRoom.Name })
        })
    });
        }

        public object GetCoursesForTeacher(int teacherId)
        {
            var teacher = db.TeachersRepository.Get(filter: t => t.Id == teacherId, includeProperties : "Teachings,Teachings.Programs,Teachings.Programs.TakingStudents").FirstOrDefault();

            return Converters.TeachersConverter.TeacherToTeacherEvenMoreExtendedDto(teacher);
        }

        public object GetClassRoomsCoursesForTeacher(int teacherId)
        {
            throw new NotImplementedException();
        }

        public object GetCoursesClassRoomsForTeacher(int teacherId)
        {
            // experiments
            // I dont need everything to check out if something is ok...
            var teacher = db.TeachersRepository.Get(tr => tr.Id == teacherId).FirstOrDefault();

            var programsGrouped = teacher.Teachings.SelectMany(t => t.Programs).GroupBy(p => p.ClassRoom);

            // Ok ovo vraca spisak odeljenja
            var classrooms = programsGrouped.Select(g => new
            {
                ClassRoomId = g.Key.Id,
                ClassName = g.Key.Name
            });

            // da probam opet -- izgleda da je ovo to...
            var classroomsAndCourses = programsGrouped.Select(g => new
            {
                ClassRoomId = g.Key.Id,
                ClassName = g.Key.Name,
                Courses = g.Select(prog => new
                {
                    CourseId = prog.Course.Id,
                    CourseName = prog.Course.Name,
                    TeacherId = prog.Teaching.TeacherId,
                    TeacherName = prog.Teaching.Teacher.FirstName
                })
            });


            return null;
        }

        /// <summary>
        /// Get a teacher by Id
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public TeacherUser GetTeacherById(int teacherId)
        {
            TeacherUser teacher = db.TeachersRepository.Get(t => t.Id == teacherId).FirstOrDefault();

            if (teacher == null)
            {
                logger.Info("Teacher {@teacherId} not found", teacherId);
                var ex = new TeacherNotFoundException(string.Format("Teacher {0} not found", teacherId));
                ex.Data.Add("teacherId", teacherId);
                throw ex;
            }

            return teacher;
        }

        public TeacherReportDto GetTeacherReport(int teacherId)
        {
            TeacherUser teacher = GetTeacherById(teacherId);

            return Converters.TeachersConverter.TeacherToTeacherReportDto(teacher);
        }
    }
}