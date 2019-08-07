using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Courses;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Models.Dtos.Teachings;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
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
        private ITakingsService takingsService;
        private IProgramsService programsService;
        private ITeachingsService teachingsService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public CoursesService(IUnitOfWork db, ILogger logger, 
            ITakingsService takingsService, 
            IProgramsService programsService,
            ITeachingsService teachingsService)
        {
            this.db = db;
            this.logger = logger;
            this.takingsService = takingsService;
            this.programsService = programsService;
            this.teachingsService = teachingsService;
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
                return null;
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
                Id = newCourse.Id
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
            throw new NotImplementedException();
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

        public TeachingDto GetTeaching(int courseId, int teacherId)
        {
            // I probably dont need these. Just return null if there's no teaching
            Course course = db.CoursesRepository.GetByID(courseId);

            if (course == null)
            {
                return null;
            }

            TeacherUser teacher = db.TeachersRepository.Get(t => t.Id == teacherId).FirstOrDefault();

            if (teacher == null)
            {
                return null;
            }

            Teaching teaching = db.TeachingAssignmentsRepository.Get(t => t.CourseId== courseId && t.TeacherId == teacherId).FirstOrDefault();

            if (teaching == null)
            {
                return null;
            }

            return TeachingsConverter.TeachingToTeachingDto(teaching);
        }

        public IEnumerable<TeachingDto> GetAllTeachings(int courseId)
        {
            var teachings = db.TeachingAssignmentsRepository.Get(t => t.CourseId == courseId)
                .Select(t => Converters.TeachingsConverter.TeachingToTeachingDto(t));

            return teachings;
        }

        public TeachingDto CreateTeachingAssignment(TeachingDto teaching)
        {
            //Course course = db.CoursesRepository.GetByID(teaching.CourseId);

            //if (course == null)
            //{
            //    return null;
            //}

            //TeacherUser teacher = db.TeachersRepository.Get(t => t.Id == teaching.TeacherId).FirstOrDefault();

            //if (teacher == null)
            //{
            //    return null;
            //}

            // OK.
            // Without teacher & user 
            //      1) required must be withheld
            //      2) if teacher or course id is not valid, there will be a foreign key violation
            //      3) if there is already a teaching assignment, a unique constraint will fire

            Teaching newTeaching = new Teaching()
            {
                CourseId = teaching.CourseId,
                TeacherId = teaching.TeacherId
            };

            db.TeachingAssignmentsRepository.Insert(newTeaching);
            db.Save();

            return TeachingsConverter.TeachingToTeachingDto(newTeaching);
        }

        public TeachingDto DeleteTeachingAssignment(TeachingDto teaching)
        {
            Teaching deletedTeaching = db.TeachingAssignmentsRepository.Get(t => t.CourseId == teaching.CourseId && t.TeacherId == teaching.TeacherId).FirstOrDefault();
            var dto = TeachingsConverter.TeachingToTeachingDto(deletedTeaching);
            db.TeachingAssignmentsRepository.Delete(deletedTeaching);
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

        public IEnumerable<ProgramDto> GetAllPrograms(int courseId, int teacherId)
        {
            var programs = db.ProgramsRepository
                .Get(p => p.Teaching.CourseId == courseId && p.Teaching.TeacherId == teacherId)
                .Select(p => Converters.ProgramsConverter.ProgramToProgramDto(p));

            return programs;
        }

        public ProgramDto GetProgram(int courseId, int teacherId, int classRoomId)
        {
            Program program = programsService.GetProgram(courseId, teacherId, classRoomId);

            return Converters.ProgramsConverter.ProgramToProgramDto(program);
        }

        public ProgramDto CreateProgram(ProgramDto program)
        {
            Program newProgram = programsService.CreateProgram(program);

            return Converters.ProgramsConverter.ProgramToProgramDto(newProgram);
        }

        public ProgramDto UpdateProgram(ProgramDto program)
        {
            Program updatedProgram = programsService.UpdateProgram(program);

            return Converters.ProgramsConverter.ProgramToProgramDto(updatedProgram);
        }

        public ProgramDto DeleteProgram(ProgramDto program)
        {
            Program deletedProgram = programsService.DeleteProgram(program);

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
            var taking = takingsService.GetTaking(courseId, teacherId, classRoomId, studentId);

            return TakingsConverter.TakingToTakingDto(taking);
        }

        public TakingDto CreateTaking(TakingDto taking)
        {
            Taking newTaking = takingsService.CreateTaking(taking);

            return Converters.TakingsConverter.TakingToTakingDto(newTaking);
        }

        public TakingDto DeleteTaking(TakingDto taking)
        {
            var deletedTaking = takingsService.DeleteTaking(taking);

            return TakingsConverter.TakingToTakingDto(deletedTaking);
        }
        #endregion
    }
}