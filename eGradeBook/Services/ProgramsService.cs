using eGradeBook.Models.Dtos;
using eGradeBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public class ProgramsService : IProgramsService
    {
        private IUnitOfWork db;

        public ProgramsService(IUnitOfWork db)
        {
            this.db = db;
        }

        public IEnumerable<ProgramsByCoursesDto> GetAllProgramsGroupedByCourses()
        {
            return db.ProgramsRepository.Get()
                .GroupBy(p => p.Course)
                .Select(g => new ProgramsByCoursesDto()
                {
                    CourseName = g.Key.Name,
                    ClassRooms = g.Select(gs => gs.SchoolClass.Name).ToList()
                });
        }

        public IEnumerable<ProgramsBySchoolClassesDto> GetAllProgramsGroupedBySchoolClasses()
        {
            return db.ProgramsRepository.Get()
                .GroupBy(p => p.SchoolClass)
                .Select(g => new ProgramsBySchoolClassesDto()
                {
                    SchoolClass = g.Key.Name,
                    Courses = g.Select(gs => gs.Course.Name).ToList()
                });
        }
    }
}