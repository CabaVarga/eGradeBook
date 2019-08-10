using eGradeBook.Models;
using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Grades;
using eGradeBook.Repositories;
using eGradeBook.Services.Converters;
using eGradeBook.Services.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace eGradeBook.Services
{
    /// <summary>
    /// Service for everything grades related
    /// </summary>
    public class GradesService : IGradesService
    {
        private IUnitOfWork db;
        private ILogger logger;

        // All services except Grade
        private Lazy<IAdminsService> adminsService;
        private Lazy<ITeachersService> teachersService;
        private Lazy<IStudentsService> studentsService;
        private Lazy<IParentsService> parentsService;

        private Lazy<ITeachingsService> teachingsService;
        private Lazy<IProgramsService> programsService;
        private Lazy<ITakingsService> takingsService;
        private Lazy<IEmailsService> emailsService;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="logger"></param>
        public GradesService(IUnitOfWork db, 
            ILogger logger,
            Lazy<IAdminsService> adminsService,
            Lazy<ITeachersService> teachersService,
            Lazy<IStudentsService> studentsService,
            Lazy<IParentsService> parentsService,
            Lazy<ITeachingsService> teachingsService,
            Lazy<IProgramsService> programsService,
            Lazy<ITakingsService> takingsService,
            Lazy<IEmailsService> emailsService)
        {
            this.db = db;
            this.logger = logger;
            this.adminsService = adminsService;
            this.teachersService = teachersService;
            this.studentsService = studentsService;
            this.parentsService = parentsService;
            this.teachingsService = teachingsService;
            this.programsService = programsService;
            this.takingsService = takingsService;
            this.emailsService = emailsService;
        }

        /// <summary>
        /// Create grade with params
        /// </summary>
        /// <returns></returns>
        public Grade CreateGrade(
            int courseId, int teacherId, int classRoomId, int studentId,
            int schoolTerm, DateTime assigned,
            int gradePoint, string notes = null)
        {
            // Or go for taking directly!
            // It will eliminate any anomaly down the road
            Taking taking = takingsService.Value.GetTaking(courseId, teacherId, classRoomId, studentId);

            // School Term (or semester) check
            // For that we need a schoolYear with firstTermStartDate, firstTermEndDate, secondTermStartDate and secondTermEndDate
            // Not impossible, but quite unlikely that I will implement that until Saturday...

            // Final Grade check (if there is a final grade for a given semester, you cant assign a new grade
            // also: without final grade for first semester cant assign a grade into the new semester...

            if (taking.FinalGrades.Count() > 0)
            {
                // check this stuff here...
            }


            Grade grade = new Grade()
            {
                GradePoint = gradePoint,
                Taking = taking,                
                Assigned = assigned,
                Notes = notes,
                SchoolTerm = schoolTerm
            };

            db.GradesRepository.Insert(grade);
            db.Save();

            return grade;
        }
        /// <summary>
        /// Create grade from gradeDto
        /// </summary>
        /// <param name="gradeDto"></param>
        /// <returns></returns>
        public Grade CreateGrade(GradeDto gradeDto)
        {
            logger.Info("Create grade {@gradeData}", gradeDto);

            var grade = CreateGrade(
                gradeDto.CourseId, gradeDto.TeacherId, gradeDto.ClassRoomId, gradeDto.StudentId,
                gradeDto.Semester, gradeDto.AssignmentDate, gradeDto.GradePoint, gradeDto.Notes);

            // NOTIFY PARENTS

            // ----- TURN OFF TEMPORARILY WHILE TESTING....
            // emailsService.Value.NotifyParents(grade);

            return grade;

        }

        /// <summary>
        /// Create grade from GradeDto return dto
        /// </summary>
        /// <param name="gradeDto"></param>
        /// <returns></returns>
        public GradeDto CreateGradeDto(GradeDto gradeDto)
        {
            logger.Info("Create grade {@gradeData} return dto", gradeDto);

            var grade = CreateGrade(gradeDto);

            return Converters.GradesConverter.GradeToGradeDto(grade);
        }

        /// <summary>
        /// Retrieve all grades in the system
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GradeDto> GetAllGrades()
        {
            // This is the most basic implementation, with every grade included, no ordering, no grouping..
            var grades = db.GradesRepository.Get()
                .Select(g => GradesConverter.GradeToGradeDto(g));

            return grades;
        }

        /// <summary>
        /// Retrieve all grades assigned by the given teacher
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public IEnumerable<GradeDto> GetAllGradesForTeacher(int teacherId)
        {
            // Probably need to check the teacher...

            var grades = db.GradesRepository.Get(
                filter: g => g.Taking.Program.Teaching.TeacherId == teacherId)
                .Select(g => GradesConverter.GradeToGradeDto(g));

            return grades;
        }

        /// <summary>
        /// Retrieve all grades for a given student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public IEnumerable<GradeDto> GetAllGradesForStudent(int studentId)
        {
            var grades = db.GradesRepository.Get(
                filter: g => g.Taking.StudentId == studentId)
                .Select(g => GradesConverter.GradeToGradeDto(g));

            return grades;
        }

        /// <summary>
        /// Retrieve all grades for a given parent
        /// It will call and retrieve grades for parent's children
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public IEnumerable<GradeDto> GetAllGradesForParent(int parentId)
        {
            var children = db.ParentsRepository.GetByID(parentId).StudentParents.Select(sp => sp.Student);
            var childrenIds = new List<int>();

            foreach (var c in children)
            {
                childrenIds.Add(c.Id);
            }

            // Hmm... i'm not quite it will work...
            var grades = db.GradesRepository.Get(g => g.Taking.Student.StudentParents.Any(p => p.Parent.Id == parentId))
                .Select(g => GradesConverter.GradeToGradeDto(g));

            return grades;
        }

        /// <summary>
        /// Retrieve all grades for a given course
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GradeDto> GetGradesByCourses()
        {
            // NOTE misleading title. no grouping at all
            var grades = db.GradesRepository.Get()
                .Select(g => GradesConverter.GradeToGradeDto(g));

            return grades;
        }

        /// <summary>
        /// Retrieve grades by multiple parameters.
        /// Specialized methods can call this method.
        /// If the output format needs to be changed, we can supply a converter delegate, maybe?
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="gradeId"></param>
        /// <param name="teacherId"></param>
        /// <param name="courseId"></param>
        /// <param name="semesterId"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        public IEnumerable<GradeDto> GetGradesByParameters(
            int? gradeId = null,
            int? courseId = null, 
            int? teacherId = null, 
            int? classRoomId = null, 
            int? studentId = null,
            int? parentId = null,
            int? semester = null, 
            int? schoolGrade = null,
            int? grade = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            
            //Func<Grade, bool> filter =
            //    g => studentId != null ? g.Taking.StudentId == studentId : true &&
            //        gradeId != null ? g.Taking.Program.ClassRoom.ClassGrade == gradeId : true &&
            //        teacherId != null ? g.Taking.Program.Teaching.TeacherId == teacherId : true &&
            //        courseId != null ? g.Taking.Program.CourseId == courseId : true &&
            //        semesterId != null ? g.SchoolTerm == semesterId : true &&
            //        classId != null ? g.Taking.Program.ClassRoomId == classId : true;

            return db.GradesRepository.Get(
                filter:
                g => (gradeId != null ? g.Id == gradeId : true) &&
                    (courseId != null ? g.Taking.Program.CourseId == courseId : true) &&
                    (teacherId != null ? g.Taking.Program.Teaching.TeacherId == teacherId : true) &&
                    (classRoomId != null ? g.Taking.Program.ClassRoomId == classRoomId : true) &&
                    (studentId != null ? g.Taking.StudentId == studentId : true) &&
                    (parentId != null ? g.Taking.Student.StudentParents.Any(sp => sp.ParentId == parentId) : true) &&
                    (semester != null ? g.SchoolTerm == semester : true) &&
                    (schoolGrade != null ? g.Taking.Program.ClassRoom.ClassGrade == schoolGrade : true) &&
                    (grade != null ? g.GradePoint == grade : true) &&
                    (fromDate != null ? g.Assigned >= fromDate : true) &&
                    (toDate != null ? g.Assigned <= toDate : true))
                    .Select(g => GradesConverter.GradeToGradeDto(g));

        }

        /// <summary>
        /// Get grade by Id
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public Grade GetGradeById(int gradeId)
        {
            logger.Info("Get grade by Id {@gradeId}", gradeId);

            var grade = db.GradesRepository.GetByID(gradeId);

            if (grade == null)
            {
                return null;
            }

            return grade;
        }

        /// <summary>
        /// Get grade by Id as dto
        /// </summary>
        /// <param name="gradeId"></param>
        /// <returns></returns>
        public GradeDto GetGradeDtoById(int gradeId)
        {
            logger.Info("Get grade dto by Id {@gradeId}", gradeId);

            Grade grade = GetGradeById(gradeId);

            return Converters.GradesConverter.GradeToGradeDto(grade);
        }

        /// <summary>
        /// Get grades by parameters provided in the queryDto
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<GradeDto> GetGradesByParameters(GradeQueryDto query)
        {
            return GetGradesByParameters(
                query.GradeId,
                query.CourseId, query.TeacherId, query.ClassRoomId, query.StudentId, 
                query.ParentId, query.Semester, query.SchoolGrade, query.Grade, query.FromDate, query.ToDate);
        }
    }
}