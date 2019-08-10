using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Teachings;
using eGradeBook.Repositories;
using eGradeBook.Services.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with Program entities -- the combination of classroom, course and teacher
    /// NOTE Used by the programs controller for now
    /// Can remain, other services, takings and grade can call it
    /// </summary>
    public class ProgramsService : IProgramsService
    {
        private IUnitOfWork db;
        private ILogger logger;
        private Lazy<ITeachingsService> teachingsService;
        private Lazy<IClassRoomsService> classRoomsService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        public ProgramsService(IUnitOfWork db, ILogger logger, 
            Lazy<ITeachingsService> teachingsService,
            Lazy<IClassRoomsService> classRoomsService)
        {
            this.db = db;
            this.logger = logger;
            this.teachingsService = teachingsService;
            this.classRoomsService = classRoomsService;
        }

        /// <summary>
        /// Create program from dto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        public Program CreateProgram(ProgramDto programDto)
        {
            logger.Info("Create program {@programDto}", programDto);
            return CreateProgram(programDto.CourseId, programDto.TeacherId, programDto.ClassRoomId, programDto.WeeklyHours);
        }

        public Program CreateProgram(int courseId, int teacherId, int classRoomId, int weeklyHours)
        {
            logger.Info("Create program for course {@courseId}, teacher {@teacherId} and classroom {@classRoomId}", courseId, teacherId, classRoomId);
            // We need a teaching
            Teaching teaching = teachingsService.Value.GetTeaching(courseId, teacherId);
            ClassRoom classRoom = db.ClassRoomsRepository.Get(c => c.Id == classRoomId).FirstOrDefault();

            Program program = new Program()
            {
                Teaching = teaching,
                ClassRoom = classRoom,
                WeeklyHours = weeklyHours,
                Course = teaching.Course
            };

            db.ProgramsRepository.Insert(program);
            db.Save();

            return program;
        }

        /// <summary>
        /// Get program for a given dto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        public Program GetProgram(ProgramDto programDto)
        {
            logger.Info("Get program {@programDto}", programDto);
            return GetProgram(programDto.CourseId, programDto.TeacherId, programDto.ClassRoomId);
        }

        /// <summary>
        /// Get program for course, teacher and classroom
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        public Program GetProgram(int courseId, int teacherId, int classRoomId)
        {
            logger.Info("Get program for course {@courseId}, teacher {@teacherId} and classroom {@classRoomId}", courseId, teacherId, classRoomId);

            Teaching teaching = teachingsService.Value.GetTeaching(courseId, teacherId);

            ClassRoom classRoom = classRoomsService.Value.GetClassRoom(classRoomId);

            var program = db.ProgramsRepository.Get(p =>
                    p.ClassRoomId == classRoomId &&
                    p.Teaching.CourseId == courseId &&
                    p.Teaching.TeacherId == teacherId)
                .FirstOrDefault();

            if (program == null)
            {
                logger.Info("Program not found for course {@courseId}, teacher {@teacherId} and classroom {@classRoomId}", courseId, teacherId, classRoomId);
                var ex = new ProgramNotFoundException(string.Format("Program not found for course {0}, teacher {1} and classroom {2}", courseId, teacherId, classRoomId));
                ex.Data.Add("teacherId", teacherId);
                ex.Data.Add("courseId", courseId);
                ex.Data.Add("classRoomId", classRoomId);
                throw ex;
            }

            return program;
        }

        /// <summary>
        /// Delete program
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        public Program DeleteProgram(ProgramDto programDto)
        {
            return DeleteProgram(programDto.CourseId, programDto.TeacherId, programDto.ClassRoomId);
        }

        /// <summary>
        /// Delete program
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        public Program DeleteProgram(int courseId, int teacherId, int classRoomId)
        {
            Program program = GetProgram(courseId, teacherId, classRoomId);

            db.ProgramsRepository.Delete(program);
            db.Save();

            return program;
        }

        /// <summary>
        /// Get all programs grouped by courses
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProgramsByCoursesDto> GetAllProgramsGroupedByCourses()
        {
            return db.ProgramsRepository.Get()
                .GroupBy(p => p.Course)
                .Select(g => new ProgramsByCoursesDto()
                {
                    CourseName = g.Key.Name,
                    ClassRooms = g.Select(gs => gs.ClassRoom.Name).ToList()
                });
        }

        /// <summary>
        /// Get all programs grouped by classrooms
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProgramsBySchoolClassesDto> GetAllProgramsGroupedBySchoolClasses()
        {
            return db.ProgramsRepository.Get()
                .GroupBy(p => p.ClassRoom)
                .Select(g => new ProgramsBySchoolClassesDto()
                {
                    ClassRoom = g.Key.Name,
                    Courses = g.Select(gs => gs.Course.Name).ToList()
                });
        }

        /// <summary>
        /// Update program
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        public Program UpdateProgram(ProgramDto programDto)
        {
            return UpdateProgram(programDto.CourseId, programDto.TeacherId, programDto.ClassRoomId, programDto.WeeklyHours);
        }

        /// <summary>
        /// Update program
        /// NOTE it will only update weeklyHours
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <param name="classRoomId"></param>
        /// <param name="weeklyHours"></param>
        /// <returns></returns>
        public Program UpdateProgram(int courseId, int teacherId, int classRoomId, int weeklyHours)
        {
            Program program = GetProgram(courseId, teacherId, classRoomId);

            program.WeeklyHours = weeklyHours;

            db.ProgramsRepository.Update(program);
            db.Save();

            return program;
        }

        /// <summary>
        /// Get all programs for teaching
        /// </summary>
        /// <param name="teachingDto"></param>
        /// <returns></returns>
        public IEnumerable<Program> GetAllProgramsForTeaching(TeachingDto teachingDto)
        {
            return GetAllProgramsForTeaching(teachingDto.CourseId, teachingDto.TeacherId);
        }

        /// <summary>
        /// Get all programs for teaching (course and teacher)
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public IEnumerable<Program> GetAllProgramsForTeaching(int courseId, int teacherId)
        {
            Teaching teaching = teachingsService.Value.GetTeaching(courseId, teacherId);

            return db.ProgramsRepository.Get(p => p.Teaching.CourseId == courseId && p.Teaching.TeacherId == teacherId);
        }

        /// <summary>
        /// Get program by Id
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public Program GetProgram(int programId)
        {
            logger.Info("Get program Id {@programId}", programId);
            return db.ProgramsRepository.GetByID(programId);
        }

        /// <summary>
        /// Get program by Id and return dto
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public ProgramDto GetProgramDto(int programId)
        {
            logger.Info("Get program dto Id {@programId}", programId);
            Program program = GetProgram(programId);

            return Converters.ProgramsConverter.ProgramToProgramDto(program);
        }

        /// <summary>
        /// Create program and return dto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        public ProgramDto CreateProgramDto(ProgramDto programDto)
        {
            Program program = CreateProgram(programDto);

            return Converters.ProgramsConverter.ProgramToProgramDto(program);
        }

        /// <summary>
        /// Get all programs as dtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProgramDto> GetAllProgramsDtos()
        {
            logger.Info("Get all programs as dtos");

            return db.ProgramsRepository.Get()
                .Select(p => Converters.ProgramsConverter.ProgramToProgramDto(p));
        }

        /// <summary>
        /// Update program and return dto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        public ProgramDto UpdateProgramDto(ProgramDto programDto)
        {
            logger.Info("Update program {@programData} and return dto", programDto);
            Program updatedProgram = UpdateProgram(programDto);

            return Converters.ProgramsConverter.ProgramToProgramDto(updatedProgram);
        }
    }
}