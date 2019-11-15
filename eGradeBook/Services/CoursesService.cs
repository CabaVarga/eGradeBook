using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Courses;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Models.Dtos.Teachings;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
using eGradeBook.Services.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with courses and all tasks related to them
    /// </summary>
    public class CoursesService : ICoursesService
    {
        private IUnitOfWork db;
        private ILogger logger;
        private Lazy<IStudentsService> studentsService;
        private Lazy<ITakingsService> takingsService;
        private Lazy<IProgramsService> programsService;
        private Lazy<ITeachingsService> teachingsService;
        private Lazy<ITeachersService> teachersService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public CoursesService(IUnitOfWork db, ILogger logger,
            Lazy<IStudentsService> studentsService,
            Lazy<ITakingsService> takingsService,
            Lazy<IProgramsService> programsService,
            Lazy<ITeachingsService> teachingsService,
            Lazy<ITeachersService> teachersService)
        {
            this.db = db;
            this.logger = logger;
            this.takingsService = takingsService;
            this.programsService = programsService;
            this.teachingsService = teachingsService;
            this.teachersService = teachersService;
        }

        #region FULL CRUD

        /// <summary>
        /// Create a new course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public Course CreateCourse(Course course)
        {
            db.CoursesRepository.Insert(course);
            db.Save();

            return course;
        }

        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Course> GetAllCourses()
        {
            return db.CoursesRepository.Get();
        }

        /// <summary>
        /// Get a course by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Course GetCourseById(int id)
        {
            Course course = db.CoursesRepository.GetByID(id);

            if (course == null)
            {
                logger.Info("Course {@courseId} not found", id);
                var ex = new CourseNotFoundException(string.Format("Course {0} not found", id));
                ex.Data.Add("courseId", id);
                throw ex;
            }

            return course;
        }

        /// <summary>
        /// Update course
        /// </summary>
        /// <param name="id"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        public Course UpdateCourse(int id, Course course)
        {
            Course courseUpdated = db.CoursesRepository.GetByID(id);

            if (courseUpdated == null)
            {
                return null;
            }

            courseUpdated.Name = course.Name;
            courseUpdated.ColloqialName = course.ColloqialName;

            db.CoursesRepository.Update(courseUpdated);
            db.Save();

            return courseUpdated;
        }

        /// <summary>
        /// Delete course
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Course DeleteCourse(int id)
        {
            Course deletedCourse = db.CoursesRepository.GetByID(id);

            if (deletedCourse == null)
            {
                return null;
            }


            db.CoursesRepository.Delete(deletedCourse);
            db.Save();

            return deletedCourse;
        }

        #endregion

        #region Dto CRUD
        /// <summary>
        /// Create course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public CourseDto CreateCourse(CourseDto course)
        {
            logger.Info("Service received request for Course {@courseData} creation", course);

            Course newCourse = new Course()
            {
                Name = course.Name,
                ColloqialName = course.ColloqialName
            };

            // NOTE Should I inspect beforehand or just shoot
            // and handle error in case of unsuccessful insertion
            CreateCourse(newCourse);

            CourseDto courseDto = new CourseDto()
            {
                Name = newCourse.Name,
                ColloqialName = newCourse.ColloqialName,
                CourseId = newCourse.Id
            };

            logger.Info("Course creation succesful");
            return courseDto;
        }

        /// <summary>
        /// Delete course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public CourseDto DeleteCourse(CourseDto course)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        public CourseDto UpdateCourse(CourseDto course)
        {
            Course updatedCourse = GetCourseById(course.CourseId);

            updatedCourse.Name = course.Name;
            updatedCourse.ColloqialName = course.ColloqialName;

            db.CoursesRepository.Update(updatedCourse);
            db.Save();

            return CoursesConverter.CourseToCourseDto(updatedCourse);
        }

        /// <summary>
        /// Retrieve all courses and return them as CourseDtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CourseDto> GetAllCoursesDto()
        {
            logger.Trace("Service received a request to return all courses as dtos");
            return db.CoursesRepository.Get()
                .Select(c => CoursesConverter.CourseToCourseDto(c));
        }

        /// <summary>
        /// Retrieve a course by Id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public CourseDto GetCourseDtoById(int courseId)
        {
            logger.Trace("Service received a request to a course by Id {courseId}", courseId);
            var course = db.CoursesRepository.GetByID(courseId);

            if (course == null)
            {
                return null;
            }

            return CoursesConverter.CourseToCourseDto(course);
        }
        #endregion

        #region Teachings

        /// <summary>
        /// Get teaching assignment
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public TeachingDto GetTeaching(int courseId, int teacherId)
        {
            // I probably dont need these. Just return null if there's no teaching
            Course course = db.CoursesRepository.GetByID(courseId);

            // No null check here, because the teachers service takes care of that...!
            TeacherUser teacher = teachersService.Value.GetTeacherById(teacherId);
            
            // Probably teaching will get Course and TeacherUser, so failure will be there!
            Teaching teaching = teachingsService.Value.GetTeaching(courseId, teacherId);

            if (teaching == null)
            {
                return null;
            }

            return TeachingsConverter.TeachingToTeachingDto(teaching);
        }

        /// <summary>
        /// Get all teachings and return them as dtos
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public IEnumerable<TeachingDto> GetAllTeachings(int courseId)
        {
            Course course = GetCourseById(courseId);

            var teachings = db.TeachingAssignmentsRepository.Get(t => t.CourseId == courseId)
                .Select(t => Converters.TeachingsConverter.TeachingToTeachingDto(t));

            return teachings;
        }

        /// <summary>
        /// This should be the default entry for teaching assignments
        /// </summary>
        /// <param name="teaching"></param>
        /// <returns></returns>
        public TeachingDto CreateTeachingAssignment(TeachingDto teaching)
        {
            Course course = GetCourseById(teaching.CourseId);

            TeacherUser teacher = teachersService.Value.GetTeacherById(teaching.TeacherId);

            // Without teacher & user 
            //      1) required must be withheld
            //      2) if teacher or course id is not valid, there will be a foreign key violation
            //      3) if there is already a teaching assignment, a unique constraint will fire

            Teaching newTeaching = new Teaching()
            {
                Course = course,
                Teacher = teacher
            };

            db.TeachingAssignmentsRepository.Insert(newTeaching);
            db.Save();

            return TeachingsConverter.TeachingToTeachingDto(newTeaching);
        }

        /// <summary>
        /// Delete a teaching assignment
        /// </summary>
        /// <param name="teaching"></param>
        /// <returns></returns>
        public TeachingDto DeleteTeachingAssignment(TeachingDto teaching)
        {

            Teaching deletedTeaching = teachingsService.Value.DeleteTeaching(teaching);

            db.TeachingAssignmentsRepository.Delete(deletedTeaching);
            db.Save();

            var dto = TeachingsConverter.TeachingToTeachingDto(deletedTeaching);

            return dto;
        }

        /// <summary>
        /// Assign teacher to course
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        public void AssignTeacherToCourse(int teacherId, int courseId)
        {
            var user = db.TeachersRepository.GetByID(teacherId);

            if (user.GetType() != typeof(TeacherUser))
            {
                //
            }

            // ifnull also...
        }

        /// <summary>
        /// Remove teacher from course
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        public void RemoveTeacherFromCourse(int teacherId, int courseId)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Programs
        /// <summary>
        /// Get all programs for course and teacher
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public IEnumerable<ProgramDto> GetAllPrograms(int courseId, int teacherId)
        {
            var programs = programsService.Value.GetAllProgramsForTeaching(courseId, teacherId)
                .Select(p => Converters.ProgramsConverter.ProgramToProgramDto(p));

            return programs;
        }

        public ProgramDto GetProgram(int courseId, int teacherId, int classRoomId)
        {
            Program program = programsService.Value.GetProgram(courseId, teacherId, classRoomId);

            return Converters.ProgramsConverter.ProgramToProgramDto(program);
        }

        public ProgramDto CreateProgram(ProgramDto program)
        {
            Program newProgram = programsService.Value.CreateProgram(program);

            return Converters.ProgramsConverter.ProgramToProgramDto(newProgram);
        }

        public ProgramDto UpdateProgram(ProgramDto program)
        {
            Program updatedProgram = programsService.Value.UpdateProgram(program);

            return Converters.ProgramsConverter.ProgramToProgramDto(updatedProgram);
        }

        public ProgramDto DeleteProgram(ProgramDto program)
        {
            Program deletedProgram = programsService.Value.DeleteProgram(program);

            return Converters.ProgramsConverter.ProgramToProgramDto(deletedProgram);
        }
        #endregion

        #region Takings
        public IEnumerable<TakingDto> GetAllTakings(int courseId, int teacherId, int classRoomId)
        {
            // RETURNING. Where to implement? Here or in the service and convert here?
            var takings = db.TakingsRepository.Get(t => t.Program.Teaching.CourseId == courseId &&
                t.Program.Teaching.TeacherId == teacherId && t.Program.ClassRoomId == classRoomId)
                .Select(t => Converters.TakingsConverter.TakingToTakingDto(t));

            return takings;
        }

        public TakingDto GetTaking(int courseId, int teacherId, int classRoomId, int studentId)
        {
            var taking = takingsService.Value.GetTaking(courseId, teacherId, classRoomId, studentId);

            return TakingsConverter.TakingToTakingDto(taking);
        }

        public TakingDto CreateTaking(TakingDto taking)
        {
            Taking newTaking = takingsService.Value.CreateTaking(taking);

            if (newTaking == null)
            {
                return null;
            }

            return Converters.TakingsConverter.TakingToTakingDto(newTaking);
        }

        public TakingDto DeleteTaking(TakingDto taking)
        {
            var deletedTaking = takingsService.Value.DeleteTaking(taking);

            return TakingsConverter.TakingToTakingDto(deletedTaking);
        }
        #endregion

        public IEnumerable<CourseDto> GetCoursesByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var courses = db.CoursesRepository.Get(
                g => (courseId != null ? g.Id == courseId : true) &&
                    (teacherId != null ? g.Teachings.Any(t => t.TeacherId == teacherId) : true) &&
                    (classRoomId != null ? g.Teachings.Any(t => t.Programs.Any(p => p.ClassRoomId == classRoomId)) : true) &&
                    (studentId != null ? g.Teachings.Any(t => t.Programs.Any(p => p.TakingStudents.Any(ts => ts.StudentId == studentId))) : true) &&
                    (parentId != null ? g.Teachings.Any(t => t.Programs.Any(p => p.TakingStudents.Any(ts => ts.Student.StudentParents.Any(sp => sp.ParentId == parentId)))) : true) &&
                    (schoolGrade != null ? g.Teachings.Any(t => t.Programs.Any(p => p.ClassRoom.ClassGrade == schoolGrade)) : true))
                    .Select(g => Converters.CoursesConverter.CourseToCourseDto(g));

            return courses;
        }
    }
}