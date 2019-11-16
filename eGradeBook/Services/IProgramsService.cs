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
        /// <summary>
        /// Create a program from dto and return a dto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        ProgramDto CreateProgram(ProgramDto programDto);


        /// <summary>
        /// Update a program based on dto and return dto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        ProgramDto UpdateProgram(ProgramDto programDto);

        /// <summary>
        /// Get all programs as dtos
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProgramDto> GetAllPrograms();

        // I seriously need to stop making dozens of query methods
        // Make a queryable method with parameters and use the parameters
        ProgramDto GetProgramById(int programId);

        ProgramDto DeleteProgram(int programId);

        IEnumerable<ProgramDto> GetProgramsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}