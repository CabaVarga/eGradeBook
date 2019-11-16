using eGradeBook.Models;
using eGradeBook.Models.Dtos.ClassRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    /// <summary>
    /// Interface for Class Room Service
    /// </summary>
    public interface IClassRoomsService
    {
        /// <summary>
        /// Create a new classroom
        /// </summary>
        /// <param name="classRoom"></param>
        /// <returns></returns>
        ClassRoomDto CreateClassRoom(ClassRoomRegistrationDto classRoom);

        /// <summary>
        /// Get a classroom by Id
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        ClassRoomDto GetClassRoomById(int classRoomId);

        /// <summary>
        /// Get all classrooms in the system
        /// </summary>
        /// <returns></returns>
        IEnumerable<ClassRoomDto> GetAllClassRooms();

        /// <summary>
        /// Update class room data
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <param name="classRoom"></param>
        /// <returns></returns>
        ClassRoomDto UpdateClassRoom(int classRoomId, ClassRoomDto classRoom);

        /// <summary>
        /// Delete classroom
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        ClassRoomDto DeleteClassRoom(int classRoomId);







        IEnumerable<ClassRoomDto> GetClassRoomsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}