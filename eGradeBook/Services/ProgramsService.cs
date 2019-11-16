using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Teachings;
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
    /// Service for working with Program entities -- the combination of classroom, course and teacher
    /// NOTE Used by the programs controller for now
    /// Can remain, other services, takings and grade can call it
    /// </summary>
    public class ProgramsService : IProgramsService
    {
        private IUnitOfWork db;
        private ILogger logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        public ProgramsService(IUnitOfWork db, ILogger logger)
        {
            this.db = db;
            this.logger = logger;
        }



        /// <summary>
        /// Get program by Id and return dto
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        public ProgramDto GetProgramById(int programId)
        {
            logger.Info("Get program dto Id {@programId}", programId);
            Program program = db.ProgramsRepository.GetByID(programId);

            return Converters.ProgramsConverter.ProgramToProgramDto(program);
        }

        /// <summary>
        /// Create program and return dto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        public ProgramDto CreateProgram(ProgramDto programDto)
        {
            Program program = new Program()
            {
                ClassRoomId = programDto.ClassRoomId,
                TeachingId = programDto.TeachingId,
                WeeklyHours = programDto.WeeklyHours
            };

            db.ProgramsRepository.Insert(program);
            db.Save();

            return Converters.ProgramsConverter.ProgramToProgramDto(program);
        }

        /// <summary>
        /// Get all programs as dtos
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProgramDto> GetAllPrograms()
        {
            logger.Info("Get all programs as dtos");

            return db.ProgramsRepository.Get()
                .Select(p => Converters.ProgramsConverter.ProgramToProgramDto(p));
        }

        /// <summary>
        /// Update program and return dto
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        public ProgramDto UpdateProgram(ProgramDto programDto)
        {
            logger.Info("Update program {@programData} and return dto", programDto);
            Program updatedProgram = db.ProgramsRepository.GetByID(programDto);
            updatedProgram.WeeklyHours = programDto.WeeklyHours;
            db.ProgramsRepository.Update(updatedProgram);
            db.Save();

            return Converters.ProgramsConverter.ProgramToProgramDto(updatedProgram);
        }

        public IEnumerable<ProgramDto> GetProgramsByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {
            var programs = db.ProgramsRepository.Get(
                g => (courseId != null ? g.Teaching.CourseId == courseId : true) &&
                    (teacherId != null ? g.Teaching.TeacherId == teacherId : true) &&
                    (classRoomId != null ? g.ClassRoomId == classRoomId : true) &&
                    (studentId != null ? g.Takings.Any(tk => tk.Enrollment.StudentId == studentId) : true) &&
                    (parentId != null ? g.Takings.Any(tk => tk.Enrollment.Student.StudentParents.Any(sp => sp.ParentId == parentId)) : true) &&
                    (schoolGrade != null ? g.ClassRoom.ClassGrade == schoolGrade : true))
                    .Select(g => Converters.ProgramsConverter.ProgramToProgramDto(g));

            return programs;
        }

        public ProgramDto DeleteProgram(int programId)
        {
            Program program = db.ProgramsRepository.GetByID(programId);

            if (program == null)
            {
                return null;
            }

            var deletedProgram = Converters.ProgramsConverter.ProgramToProgramDto(program);

            db.ProgramsRepository.Delete(program);
            db.Save();

            return deletedProgram;
        }
    }
}