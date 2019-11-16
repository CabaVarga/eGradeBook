using eGradeBook.Models.Dtos.Enrollments;
using System.Collections.Generic;

namespace eGradeBook.Services
{
    public interface IEnrollmentsService
    {
        EnrollmentDto CreateEnrollment(EnrollmentDto enrollmentDto);
        EnrollmentDto GetEnrollmentById(int enrollmentId);
        IEnumerable<EnrollmentDto> GetEnrollments();
        EnrollmentDto UpdateEnrollment(int enrollmentId, EnrollmentDto enrollmentDto);
        EnrollmentDto DeleteEnrollment(int enrollmentId);

        IEnumerable<EnrollmentDto> GetEnrollmentsByQuery(
            int? teacherId = null,
            int? studentId = null,
            int? parentId = null,
            int? courseId = null,
            int? classRoomId = null,
            int? schoolGrade = null);
    }
}