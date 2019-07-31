using eGradeBook.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Interface for programs service
    /// Program = course + teacher + classroom ( + schoolgrade, implied by classroom)
    /// </summary>
    public interface IProgramsService
    {
        /// <summary>
        /// Get all programs, group them by courses
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProgramsByCoursesDto> GetAllProgramsGroupedByCourses();

        /// <summary>
        /// Get all programs, group them by school classes
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProgramsBySchoolClassesDto> GetAllProgramsGroupedBySchoolClasses();
    }
}