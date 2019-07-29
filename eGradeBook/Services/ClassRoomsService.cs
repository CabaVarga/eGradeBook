using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.ClassRooms;
using eGradeBook.Repositories;

namespace eGradeBook.Services
{
    public class ClassRoomsService : IClassRoomsService
    {
        private IUnitOfWork db;

        public ClassRoomsService(IUnitOfWork db)
        {
            this.db = db;
        }

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

        public IEnumerable<ClassRoomDto> GetAllClassRooms()
        {
            return db.ClassRoomsRepository.Get().Select(c => new ClassRoomDto()
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
            });
        }

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