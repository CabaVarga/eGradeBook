using eGradeBook.Models;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Takings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface ITakingsService
    {
        #region Full CRUD
        Taking CreateTaking(TakingDto takingDto);
        Taking CreateTaking(int courseId, int teacherId, int classRoomId, int studentId);
        Taking GetTaking(TakingDto takingDto);
        Taking GetTaking(int courseId, int teacherId, int classRoomId, int studentId);

        Taking GetTakingById(int takingId);
        TakingDto GetTakingDtoById(int takingId);

        TakingDto CreateTakingDto(TakingDto takingDto);

        IEnumerable<TakingDto> GetAllTakingsDtos();

        TakingDto UpdateTaking(int takingId, TakingDto takingDto);

        IEnumerable<Taking> GetAllTakings();
        IEnumerable<Taking> GetAllTakingsForProgram(ProgramDto programDto);
        IEnumerable<Taking> GetAllTakingsForProgram(int courseId, int teacherId, int classRoomId);
        IEnumerable<Taking> GetAllTakingsForStudent(int studentId);
        Taking DeleteTaking(TakingDto takingDto);
        Taking DeleteTaking(int courseId, int teacherId, int classRoomId, int studentId);
        TakingDto DeleteTakingById(int takingId);
        #endregion

        IEnumerable<TakingDto> GetTakingsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}