using eGradeBook.Models.Dtos;
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        public ProgramsService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
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
    }
}