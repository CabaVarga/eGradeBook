using eGradeBook.Models.Dtos;
using eGradeBook.Repositories;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public class ProgramsService : IProgramsService
    {
        private IUnitOfWork db;
        private ILogger logger;

        public ProgramsService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

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

        public IEnumerable<ProgramsBySchoolClassesDto> GetAllProgramsGroupedBySchoolClasses()
        {
            return db.ProgramsRepository.Get()
                .GroupBy(p => p.ClassRoom)
                .Select(g => new ProgramsBySchoolClassesDto()
                {
                    SchoolClass = g.Key.Name,
                    Courses = g.Select(gs => gs.Course.Name).ToList()
                });
        }
    }
}