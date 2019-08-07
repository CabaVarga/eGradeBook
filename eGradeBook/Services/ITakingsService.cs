using eGradeBook.Models;
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
        Taking DeleteTaking(TakingDto takingDto);
        Taking DeleteTaking(int courseId, int teacherId, int classRoomId, int studentId);
        #endregion
    }
}