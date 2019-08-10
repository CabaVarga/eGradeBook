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
    /// <summary>
    /// Takings service
    /// </summary>
    public class TakingsService : ITakingsService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private IProgramsService programsService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="programsService"></param>
        public TakingsService(IUnitOfWork db, IProgramsService programsService)
        {
            this.db = db;
            this.programsService = programsService;
        }

        /// <summary>
        /// Create taking from dto
        /// </summary>
        /// <param name="takingDto"></param>
        /// <returns></returns>
        public Taking CreateTaking(TakingDto takingDto)
        {
            logger.Info("Create taking from dto {@takingDto}", takingDto);

            return CreateTaking(takingDto.CourseId, takingDto.TeacherId, takingDto.ClassRoomId, takingDto.StudentId);
        }

        /// <summary>
        /// Create taking from course, teacher, classroom and student
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <param name="classRoomId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public Taking CreateTaking(int courseId, int teacherId, int classRoomId, int studentId)
        {
            logger.Info("Create taking for course {@courseId}, teacher {@teacherId}, classroom {@classRoomId} and student {@studentId}",
                courseId, teacherId, classRoomId, studentId);
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
            if (student.Enrollments.FirstOrDefault()?.ClassRoom.Id  != classRoomId)
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
            logger.Info("Get taking {@takingDto}", takingDto);

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
            // A more involved approach would take it apart one-by-one ...

            Program program = programsService.GetProgram(courseId, teacherId, classRoomId);

            logger.Info("Get taking for course {@courseId}, teacher {@teacherId}, classroom {@classRoomId} and student {@studentId}",
                courseId, teacherId, classRoomId, studentId);
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

        /// <summary>
        /// Delete taking
        /// </summary>
        /// <param name="takingDto"></param>
        /// <returns></returns>
        public Taking DeleteTaking(TakingDto takingDto)
        {
            logger.Info("Delete taking {@takingData}", takingDto);

            return DeleteTaking(takingDto.CourseId, takingDto.TeacherId, takingDto.ClassRoomId, takingDto.StudentId);
        }

        /// <summary>
        /// Delete taking
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <param name="classRoomId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public Taking DeleteTaking(int courseId, int teacherId, int classRoomId, int studentId)
        {
            logger.Info("Delete taking for course {@courseId}, teacher {@teacherId}, classroom {@classRoomId} and student {@studentId}",
                courseId, teacherId, classRoomId, studentId);

            Taking taking = GetTaking(courseId, teacherId, classRoomId, studentId);
            db.TakingsRepository.Delete(taking);
            db.Save();
            return taking;
        }

        /// <summary>
        /// Get all takings
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Taking> GetAllTakings()
        {
            return db.TakingsRepository.Get();
        }

        /// <summary>
        /// Get all takings for program
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        public IEnumerable<Taking> GetAllTakingsForProgram(ProgramDto programDto)
        {
            logger.Info("Get all takings for program {@programData}", programDto);

            return GetAllTakingsForProgram(programDto.CourseId, programDto.TeacherId, programDto.ClassRoomId);
        }

        /// <summary>
        /// Get all takings for program
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="teacherId"></param>
        /// <param name="classRoomId"></param>
        /// <returns></returns>
        public IEnumerable<Taking> GetAllTakingsForProgram(int courseId, int teacherId, int classRoomId)
        {
            logger.Info("Get all takings for course {@courseId}, teacher {@teacherId} and classroom {@classRoomId}",
                courseId, teacherId, classRoomId);

            Program program = programsService.GetProgram(courseId, teacherId, classRoomId);

            return db.TakingsRepository.Get(t =>
                    t.Program.ClassRoomId == classRoomId &&
                    t.Program.Teaching.CourseId == courseId &&
                    t.Program.Teaching.TeacherId == teacherId);
        }

        /// <summary>
        /// Get all takings by student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public IEnumerable<Taking> GetAllTakingsForStudent(int studentId)
        {
            logger.Info("Get all takings for student {@studentId}", studentId);

            var takings = db.TakingsRepository.Get(t => t.StudentId == studentId);

            return takings;
        }

        /// <summary>
        /// Get taking by Id -- not throws
        /// </summary>
        /// <param name="takingId"></param>
        /// <returns></returns>
        public Taking GetTakingById(int takingId)
        {
            logger.Info("Get taking by Id {@takingId}", takingId);

            return db.TakingsRepository.GetByID(takingId);
        }

        /// <summary>
        /// Get taking by Id return dto
        /// </summary>
        /// <param name="takingId"></param>
        /// <returns></returns>
        public TakingDto GetTakingDtoById(int takingId)
        {
            logger.Info("Get taking dto by Id {@takingId}", takingId);

            Taking taking = GetTakingById(takingId);

            return Converters.TakingsConverter.TakingToTakingDto(taking);
        }

        /// <summary>
        /// Create taking from taking dto
        /// </summary>
        /// <param name="takingDto"></param>
        /// <returns></returns>
        public TakingDto CreateTakingDto(TakingDto takingDto)
        {
            logger.Info("Create taking {@takingDto} return dto", takingDto);
            Taking taking = CreateTaking(takingDto);

            return Converters.TakingsConverter.TakingToTakingDto(taking);
        }

        /// <summary>
        /// Get all takings and return dtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TakingDto> GetAllTakingsDtos()
        {
            logger.Info("Get all takings and return dtos");

            var takings = GetAllTakings().Select(t => Converters.TakingsConverter.TakingToTakingDto(t));

            return takings;
        }
    }
}