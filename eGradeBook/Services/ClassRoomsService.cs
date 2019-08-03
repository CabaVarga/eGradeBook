using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.ClassRooms;
using eGradeBook.Repositories;
using NLog;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for working with classroom entities and related tasks
    /// </summary>
    public class ClassRoomsService : IClassRoomsService
    {
        private IUnitOfWork db;
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        public ClassRoomsService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }

        /// <summary>
        /// Create classroom
        /// </summary>
        /// <param name="classRoom"></param>
        /// <returns></returns>
        public ClassRoomDto CreateClassRoom(ClassRoomRegistrationDto classRoom)
        {
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

        public void CreateClassRoomProgram(ClassRoomProgramDto program)
        {
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

            Teaching teaching = db.TeachingAssignmentsRepository.Get(ta => ta.SubjectId == program.CourseId && ta.TeacherId == program.TeacherId).FirstOrDefault();

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
                WeeklyFund = program.WeeklyHours
            };

            db.ProgramsRepository.Insert(newProgram);
            db.Save();
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
            return db.ClassRoomsRepository.Get(cr => cr.Id == classRoomId)
                .Select(c => new ClassRoomDto()
                {
                    ClassRoomId = c.Id,
                    Name = c.Name,
                    SchoolGrade = c.ClassGrade,
                    Students = c.Students?.Select(s => new ClassRoomDto.ClassRoomStudentDto()
                    {
                        FullName = s.FirstName + " " + s.LastName,
                        StudentId = s.Id
                    }).ToList(),
                    Program = c.Program?.Select(p => new ClassRoomDto.ClassRoomProgramDto()
                    {
                        Course = p.Course.Name,
                        Teacher = p.Teaching.Teacher.FirstName + " " + p.Teaching.Teacher.LastName,
                        CourseId = p.CourseId,
                        TeacherId = p.Teaching.TeacherId
                    }).ToList()
                })
                .FirstOrDefault();               
        }
    }
}