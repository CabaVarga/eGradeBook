using eGradeBook.Models.Dtos.Students;
using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Services;
using eGradeBook.Utilities.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Web api controller for student users
    /// </summary>
    [RoutePrefix("api/students")]
    [Authorize(Roles = "admins,students,teachers,parents")]
    public class StudentsController : ApiController
    {
        private IStudentsService service;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public StudentsController(IStudentsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Get all students
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetAllStudents()
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);

            logger.Info("User {@user} requested all students",
                IdentityHelper.GetLoggedInUser(userData));

            return Ok(service.GetAllStudentsDto());
        }

        /// <summary>
        /// Get a student by Id
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(StudentDto))]
        [Route("{studentId}", Name = "GetStudentById")]
        public IHttpActionResult GetStudentById(int studentId)
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);

            logger.Info("User {@user} requested student with Id {studentId}",
                IdentityHelper.GetLoggedInUser(userData), studentId);
            return Ok(service.GetStudentByIdDto(studentId));
        }

        /// <summary>
        /// Get students by their firstname
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        [Route("by-firstname/{firstName}")]
        [HttpGet]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetStudentsByFirstName(string firstName)
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);

            logger.Info("User {@user} requested students with first name {firstName}",
                IdentityHelper.GetLoggedInUser(userData), firstName);

            return Ok(service.GetStudentsByFirstNameStartingWith(firstName));
        }

        /// <summary>
        /// Get students by their lastname
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [Route("by-lastname/{lastName}")]
        [ResponseType(typeof(IEnumerable<StudentDto>))]
        [Authorize(Roles = "admins")]
        [HttpGet]
        public IHttpActionResult GetStudentsByLastName(string lastName)
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);
            logger.Info("User {@user} requested students with last name {lastName}",
                IdentityHelper.GetLoggedInUser(userData), lastName);

            return Ok(service.GetStudentsByLastNameStartingWith(lastName));
        }


        /// <summary>
        /// Get students with their parents
        /// </summary>
        /// <returns></returns>
        [Route("with-parents")]
        [ResponseType(typeof(IEnumerable<StudentWithParentsDto>))]
        [Authorize(Roles = "admins")]
        [HttpGet]
        public IHttpActionResult GetStudentsWithParents()
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);

            logger.Info("User {@user} requested retrieval of all students with their parents",
                IdentityHelper.GetLoggedInUser(userData));

            return Ok(service.GetAllStudentsWithParents());
        }


        /// <summary>
        /// Assign a course to the student.
        /// NOTE: the implementation will check if the course is assignable to the student.
        /// 1) You cannot assign the same course twice
        /// 2) The course must be in the classroom's program
        /// 3) The course will be teached by the assigned teacher for the classroom
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [Route("{studentId}/courses")]
        [Authorize(Roles = "admins")]
        [HttpPost]
        public IHttpActionResult PostAssignCourseToStudent(int studentId, StudentCourseDto course)
        {
            var userData = IdentityHelper.FetchUserData(RequestContext);
            logger.Info("User {@user} requested assignment of course {@assignment} to student {studentId}",
                IdentityHelper.GetLoggedInUser(userData), course, studentId);

            var taking = service.AssignCourseToStudent(course);

            if (taking == null)
            {
                return BadRequest("Course assignment failed");
            }

            return Ok(taking);
        }

        [Authorize(Roles = "admins,students,parents")]
        [Route("{studentId}/report")]
        [HttpGet]
        public IHttpActionResult GetStudentReport(int studentId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Student report for {@studentId} by {@userData}", studentId, userData);                       

            if (studentId != userData.UserId && userData.UserRole == "students")
            {
                throw new UnauthorizedAccessException("You are not allowed to access other students data");
            }

            else if (userData.UserRole == "parents")
            {
                if (!service.IsParent(studentId, (int)userData.UserId))
                {
                    throw new UnauthorizedAccessException("You are not allowed to access other parents childrens data");
                }
            }

            // Teacher is a special case, because he/she can't see other courses...

            var report = service.GetStudentReport(studentId);

            return Ok(report);
        }

        /// <summary>
        /// Get students based on query
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="studentId"></param>
        /// <param name="parentId"></param>
        /// <param name="courseId"></param>
        /// <param name="classRoomId"></param>
        /// <param name="schoolGrade"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("query")]
        public IHttpActionResult GetStudentsByQuery(
            [FromUri]int? teacherId = null,
            [FromUri]int? studentId = null,
            [FromUri]int? parentId = null,
            [FromUri]int? courseId = null,
            [FromUri]int? classRoomId = null,
            [FromUri]int? schoolGrade = null)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Students by {@userData}", userData);

            logger.Info("Get grades for logged in user --- auto dispatch");

            var userInfo = Utilities.WebApi.IdentityHelper.FetchUserData(RequestContext);

            if (userInfo.UserId == null)
            {
                return Unauthorized();
            }

            var userId = userInfo.UserId ?? 0;

            if (userInfo.IsAdmin)
            {
                var results = service.GetStudentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            else if (userInfo.IsTeacher)
            {
                if (teacherId != userInfo.UserId)
                {
                    throw new UnauthorizedAccessException();
                }

                var results = service.GetStudentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            else if (userInfo.IsStudent)
            {
                if (studentId != userInfo.UserId)
                {
                    throw new UnauthorizedAccessException();
                }

                var results = service.GetStudentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            else if (userInfo.IsParent)
            {
                if (parentId != userInfo.UserId)
                {
                    throw new UnauthorizedAccessException();
                }

                var results = service.GetStudentsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            else
            {
                logger.Error("Authenticated user with no role --- this should not happen");
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("{studentId}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(StudentDto))]
        public async Task<IHttpActionResult> DeleteStudent(int studentId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Trace("Delete Student {@studentId} by {@userData}", studentId, userData);

            var result = await service.DeleteStudent(studentId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

    }
}
