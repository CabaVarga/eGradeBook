using eGradeBook.Models.Dtos.ClassRooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public interface IClassRoomsService
    {
        ClassRoomDto CreateClassRoom(ClassRoomRegistrationDto classRoom);
        ClassRoomDto GetClassRoomById(int classRoomId);
        IEnumerable<ClassRoomDto> GetAllClassRooms();
        ClassRoomDto EnrollStudent(ClassRoomEnrollStudentDto enroll);
    }
}