using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Teachings;
using eGradeBook.Repositories;
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
        private ITeachingsService teachingsService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        public ProgramsService(IUnitOfWork db, ILogger logger, ITeachingsService teachingsService)
        {
            this.db = db;
            this.logger = logger;
            this.teachingsService = teachingsService;
        }

        public Program CreateProgram(ProgramDto programDto)
        {
            return CreateProgram(programDto.CourseId, programDto.TeacherId, programDto.ClassRoomId, programDto.WeeklyHours);
        }

        public Program CreateProgram(int courseId, int teacherId, int classRoomId, int weeklyHours)
        {
            // We need a teaching
            Teaching teaching = teachingsService.GetTeaching(courseId, teacherId);
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

        public Program GetProgram(ProgramDto programDto)
        {
            return GetProgram(programDto.CourseId, programDto.TeacherId, programDto.ClassRoomId);
        }

        public Program GetProgram(int courseId, int teacherId, int classRoomId)
        {
            return db.ProgramsRepository.Get(p =>
                    p.ClassRoomId == classRoomId &&
                    p.Teaching.CourseId == courseId &&
                    p.Teaching.TeacherId == teacherId)
                .FirstOrDefault();
        }

        public Program DeleteProgram(ProgramDto programDto)
        {
            return DeleteProgram(programDto.CourseId, programDto.TeacherId, programDto.ClassRoomId);
        }

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

        public Program UpdateProgram(ProgramDto programDto)
        {
            return UpdateProgram(programDto.CourseId, programDto.TeacherId, programDto.ClassRoomId, programDto.WeeklyHours);
        }

        public Program UpdateProgram(int courseId, int teacherId, int classRoomId, int weeklyHours)
        {
            Program program = GetProgram(courseId, teacherId, classRoomId);

            program.WeeklyHours = weeklyHours;

            db.ProgramsRepository.Update(program);
            db.Save();

            return program;
        }

        public IEnumerable<Program> GetAllProgramsForTeaching(TeachingDto teachingDto)
        {
            return GetAllProgramsForTeaching(teachingDto.CourseId, teachingDto.TeacherId);
        }

        public IEnumerable<Program> GetAllProgramsForTeaching(int courseId, int teacherId)
        {
            Teaching teaching = teachingsService.GetTeaching(courseId, teacherId);

            return db.ProgramsRepository.Get(p => p.Teaching.CourseId == courseId && p.Teaching.TeacherId == teacherId);
        }
    }
}