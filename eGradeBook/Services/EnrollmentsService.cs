using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.Enrollments;
using eGradeBook.Repositories;
using NLog;

namespace eGradeBook.Services
{
    public class EnrollmentsService : IEnrollmentsService
    {
        private IUnitOfWork db;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="db"></param>
        /// <param name="programsService"></param>
        public EnrollmentsService(IUnitOfWork db)
        {
            this.db = db;
        }

        public EnrollmentDto CreateEnrollment(EnrollmentDto enrollmentDto)
        {
            var enrollment = new Enrollment()
            {
                ClassRoomId = enrollmentDto.ClassRoomId,
                StudentId = enrollmentDto.StudentId
            };

            db.EnrollmentsRepository.Insert(enrollment);
            db.Save();

            var createdEnrollment = Converters.EnrollmentsConverter.EnrollmentToEnrollmentDto(enrollment);

            return createdEnrollment;
        }

        public EnrollmentDto DeleteEnrollment(int enrollmentId)
        {
            var enrollment = db.EnrollmentsRepository.GetByID(enrollmentId);

            if (enrollment == null)
            {
                return null;
            }

            var deletedEnrollment = Converters.EnrollmentsConverter.EnrollmentToEnrollmentDto(enrollment);

            db.EnrollmentsRepository.Delete(enrollment);
            db.Save();

            return deletedEnrollment;
        }

        public EnrollmentDto GetEnrollmentById(int enrollmentId)
        {
            var enrollment = db.EnrollmentsRepository.GetByID(enrollmentId);

            if (enrollment == null)
            {
                return null;
            }

            return Converters.EnrollmentsConverter.EnrollmentToEnrollmentDto(enrollment);
        }

        public IEnumerable<EnrollmentDto> GetEnrollments()
        {
            var enrollments = db.EnrollmentsRepository.Get();

            return enrollments.Select(e => Converters.EnrollmentsConverter.EnrollmentToEnrollmentDto(e));
        }

        public IEnumerable<EnrollmentDto> GetEnrollmentsByQuery(int? teacherId = null, int? studentId = null, int? parentId = null, int? courseId = null, int? classRoomId = null, int? schoolGrade = null)
        {

                var enrollments = db.EnrollmentsRepository.Get(
                    g => (courseId != null ? g.ClassRoom.Program.Any(p => p.Teaching.CourseId == courseId) : true) &&
                        (teacherId != null ? g.ClassRoom.Program.Any(p => p.Teaching.TeacherId == teacherId) : true) &&
                        (classRoomId != null ? g.ClassRoomId == classRoomId : true) &&
                        (studentId != null ? g.StudentId == studentId : true) &&
                        (parentId != null ? g.Student.StudentParents.Any(sp => sp.ParentId == parentId) : true) &&
                        (schoolGrade != null ? g.ClassRoom.ClassGrade == schoolGrade : true))
                        .Select(g => Converters.EnrollmentsConverter.EnrollmentToEnrollmentDto(g));

            return enrollments; 
        }

        public EnrollmentDto UpdateEnrollment(int enrollmentId, EnrollmentDto enrollmentDto)
        {
            var enrollment = db.EnrollmentsRepository.GetByID(enrollmentId);

            if (enrollment == null)
            {
                return null;
            }

            enrollment.ClassRoomId = enrollmentDto.ClassRoomId;
            enrollment.StudentId = enrollment.StudentId;

            db.EnrollmentsRepository.Update(enrollment);
            db.Save();

            return Converters.EnrollmentsConverter.EnrollmentToEnrollmentDto(enrollment);
        }
    }
}