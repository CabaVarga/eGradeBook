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

        /// <summary>
        /// Create a program from programDto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        Program CreateProgram(ProgramDto programDto);

        /// <summary>
        /// Create a program from individual components
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <param name="classRoomId"></param>
        /// <param name="weeklyHours"></param>
        /// <returns></returns>
        Program CreateProgram(int courseId, int teacherId, int classRoomId, int weeklyHours);

        /// <summary>
        /// Create a program from dto and return a dto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        ProgramDto CreateProgramDto(ProgramDto programDto);

        /// <summary>
        /// Update a program and return entity
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        Program UpdateProgram(ProgramDto programDto);

        /// <summary>
        /// Update a program based on individual components
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <param name="classRoomId"></param>
        /// <param name="weeklyHours"></param>
        /// <returns></returns>
        Program UpdateProgram(int courseId, int teacherId, int classRoomId, int weeklyHours);

        /// <summary>
        /// Update a program based on dto and return dto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        ProgramDto UpdateProgramDto(ProgramDto programDto);

        /// <summary>
        /// Get a program by Id
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        Program GetProgram(int programId);

        /// <summary>
        /// Get a program by programDto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        Program GetProgram(ProgramDto programDto);

        /// <summary>
        /// Get a program by components
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        Program GetProgram(int courseId, int teacherId, int classRoomId);

        /// <summary>
        /// Get all programs as dtos
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProgramDto> GetAllProgramsDtos();

        // Eh, also for teacher etc... where will it end?
        IEnumerable<Program> GetAllProgramsForTeaching(TeachingDto teachingDto);
        IEnumerable<Program> GetAllProgramsForTeaching(int courseId, int teacherId);

        // I seriously need to stop making dozens of query methods
        // Make a queryable method with parameters and use the parameters
        ProgramDto GetProgramDto(int programId);

        ProgramDto DeleteProgramById(int programId);
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

        IEnumerable<ProgramDto> GetProgramsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}