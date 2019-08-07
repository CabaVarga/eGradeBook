using eGradeBook.Models;
using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public class TakingsService : ITakingsService
    {
        private IUnitOfWork db;
        private IProgramsService programsService;

        public TakingsService(IUnitOfWork db, IProgramsService programsService)
        {
            this.db = db;
            this.programsService = programsService;
        }

        public Taking CreateTaking(TakingDto takingDto)
        {
            return CreateTaking(takingDto.CourseId, takingDto.TeacherId, takingDto.ClassRoomId, takingDto.StudentId);
        }

        public Taking CreateTaking(int courseId, int teacherId, int classRoomId, int studentId)
        {
            // For creation we need the student and the program.
            Program program = programsService.GetProgram(courseId, teacherId, classRoomId);
            StudentUser student = db.StudentsRepository.Get(s => s.Id == studentId).FirstOrDefault();

            // need to check classroom manually :(
            if (student.ClassRoomId != classRoomId)
            {
                // probably an exception but ok for now
                return null;
            }

            Taking taking = new Taking()
            {
                Program = program,
                Student = student
            };

            db.TakingsRepository.Insert(taking);
            db.Save();

            return taking;

        }

        public Taking GetTaking(TakingDto takingDto)
        {
            return GetTaking(takingDto.CourseId, takingDto.TeacherId, takingDto.ClassRoomId, takingDto.StudentId);
        }

        public Taking GetTaking(int courseId, int teacherId, int classRoomId, int studentId)
        {
            return db.TakingsRepository.Get(t =>
                    t.StudentId == studentId &&
                    t.Program.ClassRoomId == classRoomId &&
                    t.Program.Teaching.CourseId == courseId &&
                    t.Program.Teaching.TeacherId == teacherId)
                .FirstOrDefault();
        }

        public Taking DeleteTaking(TakingDto takingDto)
        {
            return DeleteTaking(takingDto.CourseId, takingDto.TeacherId, takingDto.ClassRoomId, takingDto.StudentId);
        }

        public Taking DeleteTaking(int courseId, int teacherId, int classRoomId, int studentId)
        {
            Taking taking = GetTaking(courseId, teacherId, classRoomId, studentId);
            db.TakingsRepository.Delete(taking);
            db.Save();
            return taking;
        }
    }
}