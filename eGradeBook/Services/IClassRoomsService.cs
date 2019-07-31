﻿using eGradeBook.Models.Dtos.ClassRooms;
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
        /// Enroll the given student in the given classroom
        /// </summary>
        /// <param name="enroll"></param>
        /// <returns></returns>
        ClassRoomDto EnrollStudent(ClassRoomEnrollStudentDto enroll);
    }
}