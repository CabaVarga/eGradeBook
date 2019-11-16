using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.ClassRooms;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
using eGradeBook.Services.Exceptions;
using NLog;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with classroom entities and related tasks
    /// </summary>
    public class ClassRoomsService : IClassRoomsService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        public ClassRoomsService(IUnitOfWork db)
        {
            this.db = db;
        }

        /// <summary>
        /// Create classroom
        /// </summary>
        /// <param name="classRoom"></param>
        /// <returns></returns>
        public ClassRoomDto CreateClassRoom(ClassRoomRegistrationDto classRoom)
        {
            logger.Trace("Service received classroom creation request {classRoomData}", classRoom);

            ClassRoom newClassRoom = new ClassRoom()
            {
                Name = classRoom.Name,
                ClassGrade = classRoom.SchoolGrade
            };

            db.ClassRoomsRepository.Insert(newClassRoom);
            db.Save();

            ClassRoomDto dto = new ClassRoomDto()
            {
                ClassRoomId = newClassRoom.Id,
                Name = newClassRoom.Name,
                SchoolGrade = newClassRoom.ClassGrade
            };

            return dto;
        }



        /// <summary>
        /// Delete a classroom
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        public ClassRoomDto DeleteClassRoom(int classRoomId)
        {
            logger.Trace("Service received a request to delete classroom {classRoomId}", classRoomId);

            ClassRoom classRoom = db.ClassRoomsRepository.GetByID(classRoomId);

            if (classRoom == null)
            {
                return null;
            }

            var deletedClassroom = ClassRoomConverter.ClassRoomToClassRoomDto(classRoom);

            db.ClassRoomsRepository.Delete(classRoom);
            db.Save();

            return deletedClassroom;
        }
                         

        /// <summary>
        /// Return a list of all classrooms
        /// TODO use converter for ClassRoomDto
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClassRoomDto> GetAllClassRooms()
        {
            var classRoomDtos =  db.ClassRoomsRepository.Get()
                .Select(c => Converters.ClassRoomConverter.ClassRoomToClassRoomDto(c));

            return classRoomDtos;
        }

        /// <summary>
        /// Retrieve a classroom by id
        /// TODO use converter for ClassRoomDto
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        public ClassRoomDto GetClassRoomById(int classRoomId)
        {
            logger.Trace("Service received request to return classroom by Id {classRoomId}", classRoomId);
            var classRoom = db.ClassRoomsRepository.GetByID(classRoomId);

            if (classRoom == null)
            {
                return null;
            }

            return ClassRoomConverter.ClassRoomToClassRoomDto(classRoom);
        }

        /// <summary>
        /// Update classroom data
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <param name="classRoom"></param>
        /// <returns></returns>
        public ClassRoomDto UpdateClassRoom(int classRoomId, ClassRoomDto classRoom)
        {
            logger.Trace("Service received a request to update classroom Id {classRoomId} with data {classRoomData}", classRoomId, classRoom);
            ClassRoom updatedClassRoom = db.ClassRoomsRepository.GetByID(classRoomId);

            if (updatedClassRoom == null)
            {
                return null;
            }

            updatedClassRoom.ClassGrade = classRoom.SchoolGrade;
            updatedClassRoom.Name = classRoom.Name;

            db.ClassRoomsRepository.Update(updatedClassRoom);
            db.Save();

            return ClassRoomConverter.ClassRoomToClassRoomDto(updatedClassRoom);
        }

        public IEnumerable<ClassRoomDto> GetClassRoomsByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var classRooms = db.ClassRoomsRepository.Get(
                g => (courseId != null ? g.Program.Any(p => p.Teaching.CourseId == courseId) : true) &&
                    (teacherId != null ? g.Program.Any(p => p.Teaching.TeacherId == teacherId) : true) &&
                    (classRoomId != null ? g.Id == classRoomId : true) &&
                    (studentId != null ? g.Enrollments.Any(e => e.StudentId == studentId) : true) &&
                    (parentId != null ? g.Enrollments.Any(e => e.Student.StudentParents.Any(sp => sp.ParentId == parentId)) : true) &&
                    (schoolGrade != null ? g.ClassGrade == schoolGrade : true))
                    .Select(g => Converters.ClassRoomConverter.ClassRoomToClassRoomDto(g));

            return classRooms;
        }
    }
}