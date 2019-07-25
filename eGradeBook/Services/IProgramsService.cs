using eGradeBook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface IProgramsService
    {
        IEnumerable<ProgramsByCoursesDto> GetAllProgramsGroupedByCourses();
        IEnumerable<ProgramsBySchoolClassesDto> GetAllProgramsGroupedBySchoolClasses();
    }
}