using eGradeBook.Models;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Repositories;
using eGradeBook.Services.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eGradeBook.Services
{
    public class TakingsService : ITakingsService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();
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

            if (program == null)
            {
                return null;
            }

            if (student == null)
            {
                return null;
            }

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

        /// <summary>
        /// Get taking
        /// </summary>
        /// <param name="takingDto"></param>
        /// <returns></returns>
        public Taking GetTaking(TakingDto takingDto)
        {
            return GetTaking(takingDto.CourseId, takingDto.TeacherId, takingDto.ClassRoomId, takingDto.StudentId);
        }

        /// <summary>
        /// Get a taking
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <param name="classRoomId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public Taking GetTaking(int courseId, int teacherId, int classRoomId, int studentId)
        {
            var taking = db.TakingsRepository.Get(t =>
                    t.StudentId == studentId &&
                    t.Program.ClassRoomId == classRoomId &&
                    t.Program.Teaching.CourseId == courseId &&
                    t.Program.Teaching.TeacherId == teacherId)
                .FirstOrDefault();

            if (taking == null)
            {
                logger.Info("Student {@studentId} is not taking the course {@courseId} with teacher {@teacherId} in classroom {@classRoomId}", studentId, courseId, teacherId, classRoomId);
                var ex = new TakingNotFoundException(string.Format("Student {0} is not taking the course {1} with teacher {2} in classroom {3}", studentId, courseId, teacherId, classRoomId));
                ex.Data.Add("courseId", courseId);
                ex.Data.Add("teacherId", teacherId);
                ex.Data.Add("classRoomId", classRoomId);
                ex.Data.Add("studentId", studentId);
                throw ex;
            }

            return taking;
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

        public IEnumerable<Taking> GetAllTakings()
        {
            return db.TakingsRepository.Get();
        }

        public IEnumerable<Taking> GetAllTakingsForProgram(ProgramDto programDto)
        {
            return GetAllTakingsForProgram(programDto.CourseId, programDto.TeacherId, programDto.ClassRoomId);
        }

        public IEnumerable<Taking> GetAllTakingsForProgram(int courseId, int teacherId, int classRoomId)
        {
            Program program = programsService.GetProgram(courseId, teacherId, classRoomId);

            return db.TakingsRepository.Get(t =>
                    t.Program.ClassRoomId == classRoomId &&
                    t.Program.Teaching.CourseId == courseId &&
                    t.Program.Teaching.TeacherId == teacherId);
        }

        public IEnumerable<Taking> GetAllTakingsForStudent(int studentId)
        {
            throw new NotImplementedException();
        }
    }
}