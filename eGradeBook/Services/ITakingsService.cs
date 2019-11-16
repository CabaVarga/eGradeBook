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
        TakingDto GetTakingById(int takingId);

        TakingDto CreateTakingDto(TakingDto takingDto);

        IEnumerable<TakingDto> GetAllTakings();

        TakingDto UpdateTaking(int takingId, TakingDto takingDto);


        TakingDto DeleteTakingById(int takingId);


        IEnumerable<TakingDto> GetTakingsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}