using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Teachings;
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
        #region Full CRUD
        // Using the program fetching from multiple places, I need these things in one place!
        // Course service may be the entry but the conversion will take place here...
        // But if CreateProgram is using get Teaching, then I will not convert programDto to teachingDto
        // So i need Ids.
        Program CreateProgram(ProgramDto programDto);
        Program CreateProgram(int courseId, int teacherId, int classRoomId, int weeklyHours);
        ProgramDto CreateProgramDto(ProgramDto programDto);

        Program UpdateProgram(ProgramDto programDto);
        Program UpdateProgram(int courseId, int teacherId, int classRoomId, int weeklyHours);
        ProgramDto UpdateProgramDto(ProgramDto programDto);

        Program GetProgram(int programId);
        Program GetProgram(ProgramDto programDto);
        Program GetProgram(int courseId, int teacherId, int classRoomId);

        IEnumerable<ProgramDto> GetAllProgramsDtos();

        // Eh, also for teacher etc... where will it end?
        IEnumerable<Program> GetAllProgramsForTeaching(TeachingDto teachingDto);
        IEnumerable<Program> GetAllProgramsForTeaching(int courseId, int teacherId);

        ProgramDto GetProgramDto(int programId);

        Program DeleteProgram(ProgramDto programDto);
        Program DeleteProgram(int courseId, int teacherId, int classRoomId);
        #endregion
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