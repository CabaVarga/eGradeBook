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
        /// Create classRoom program item.
        /// Every classRoom has a program consisting of courses taught by one teacher per course.
        /// </summary>
        /// <param name="program"></param>
        public void CreateClassRoomProgram(ClassRoomProgramDto program)
        {
            logger.Trace("Service received request for classroom program creation {programData}", program);

            // I see. Manual setup.
            ClassRoom classRoom = db.ClassRoomsRepository.Get(c => c.Id == program.ClassRoomId).FirstOrDefault();

            if (classRoom == null)
            {
                // I can throw an exception and process it in the handler at the api level
                return;
            }

            Course course = db.CoursesRepository.Get(c => c.Id == program.CourseId).FirstOrDefault();

            if (course == null)
            {
                return;
            }

            TeacherUser teacher = db.TeachersRepository.Get(t => t.Id == program.TeacherId).FirstOrDefault();

            if (teacher == null)
            {
                return;
            }

            Teaching teaching = db.TeachingAssignmentsRepository.Get(ta => ta.CourseId == program.CourseId && ta.TeacherId == program.TeacherId).FirstOrDefault();

            if (teaching == null)
            {
                // this is the special case
                // every id is ok but the teacher does not teach the course
                // if we went directly, we would not know if the problem is with one of the id's or their combo...
                return;
            }

            Program newProgram = new Program()
            {
                Course = course,
                ClassRoom = classRoom,
                Teaching = teaching,
                WeeklyHours = program.WeeklyHours
            };

            db.ProgramsRepository.Insert(newProgram);
            db.Save();
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

            db.ClassRoomsRepository.Delete(classRoom);
            db.Save();

            return ClassRoomConverter.ClassRoomToClassRoomDto(classRoom);
        }

        /// <summary>
        /// Enroll student in classroom
        /// OPTIONS: enroll in classroom, enroll in program
        /// TODO use converter for classroom
        /// </summary>
        /// <param name="enroll"></param>
        /// <returns></returns>
        public ClassRoomDto EnrollStudent(ClassRoomEnrollStudentDto enroll)
        {
            // Check if classroom is ok
            ClassRoom classRoom = db.ClassRoomsRepository.GetByID(enroll.ClassRoomId);

            if (classRoom == null)
            {
                return null;
            }

            StudentUser student = db.StudentsRepository.Get(s => s.Id == enroll.StudentId).FirstOrDefault();

            if (student == null)
            {
                return null;
            }

            // Two strategies with current model:
            // -- 1. Change ClassRoom property of student.
            // -- 2. Update Students collection of classroom.
            // -- (3.) with associative table

            // with the collection stuff we have one problem:
            // well, let's find out, dear...

            var enrolled = classRoom.Students;

            // Can we check like this:
            if (enrolled.Where(e => e.Id == student.Id).FirstOrDefault() != null)
            {
                // student is already enrolled, quit
                return null;
            }

            enrolled.Add(student);

            db.Save();

            // This is only the classroom with the list of enrolled students
            // no courses.
            return db.ClassRoomsRepository.Get(c => c.Id == classRoom.Id)
                .Select(cr => new ClassRoomDto()
                {
                    ClassRoomId = cr.Id,
                    Name = cr.Name,
                    Program = null,
                    SchoolGrade = cr.ClassGrade,
                    Students = cr.Students.Select(s => new ClassRoomDto.ClassRoomStudentDto()
                    {
                        FullName = s.FirstName + " " + s.LastName,
                        StudentId = s.Id
                    }).ToList()
                })
                .FirstOrDefault();
        }

        /// <summary>
        /// Return a list of all classrooms
        /// TODO use converter for ClassRoomDto
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClassRoomDto> GetAllClassRooms()
        {
            return db.ClassRoomsRepository.Get()
                .Select(c => Converters.ClassRoomConverter.ClassRoomToClassRoomDto(c));
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

        public ClassRoom GetClassRoom(int classRoomId)
        {
            ClassRoom classRoom = db.ClassRoomsRepository.GetByID(classRoomId);

            if (classRoom == null)
            {
                logger.Info("ClassRoom {@classRoomId} not found", classRoomId);
                var ex = new ClassRoomNotFoundException(string.Format("ClassRoom {0} not found", classRoomId));
                ex.Data.Add("classRoomId", classRoomId);
                throw ex;
            }

            return classRoom;
        }
    }
}