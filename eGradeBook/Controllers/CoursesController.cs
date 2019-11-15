﻿using eGradeBook.Models.Dtos;
using eGradeBook.Models.Dtos.Courses;
using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Models.Dtos.Takings;
using eGradeBook.Models.Dtos.Teachings;
using eGradeBook.Services;
using eGradeBook.Utilities.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Web api for working with Courses
    /// </summary>
    [RoutePrefix("api/courses")]
    [Authorize]
    public class CoursesController : ApiController
    {
        private ICoursesService service;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public CoursesController(ICoursesService service)
        {
            this.service = service;
    }

        /// <summary>
        /// Get all courses
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpGet]
        //[Authorize(Roles = "admins")]
        [ResponseType(typeof(IEnumerable<CourseDto>))]
        public IHttpActionResult GetAllCourses()
        {
            var user = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get all Courses by {@userData}", user);

            var courses = service.GetAllCoursesDto();

            // TODO this is not right
            if (courses == null)
            {
                return NotFound();
            }

            return Ok(courses);
        }

        /// <summary>
        /// Get a course by Id
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [Route("{courseId:int}")]
        //[Authorize(Roles = "admins")]
        [HttpGet]
        [ResponseType(typeof(CourseDto))]
        public IHttpActionResult GetCourseById(int courseId)
        {
            var user = IdentityHelper.GetLoggedInUser(RequestContext);
            logger.Info("Get Course {@courseId} by {@userData}", courseId, user);

            var course = service.GetCourseDtoById(courseId);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        /// <summary>
        /// Register a new course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        //[Authorize(Roles = "admins")]
        [Route("")]
        [HttpPost]
        public IHttpActionResult RegisterCourse(CourseDto course)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Register Course {@courseData} by {@userData}", course, userData);

            // TODO logging
            var createdCourse = service.CreateCourse(course);

            var link = Url.Route("DefaultApi", new { controller = "Courses", courseId = createdCourse.CourseId });

            logger.Info("Course {@courseId} created at route {@route}", createdCourse.CourseId, link);

            // TODO ERROR HANDLING
            return CreatedAtRoute("DefaultApi", new { controller = "Courses", courseId = createdCourse.CourseId }, createdCourse);
        }

        [Route("{courseId}")]
        [HttpPut]
        public IHttpActionResult UpdateCourse(int courseId, CourseDto course)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Change Course {@courseData} by {@userData}", course, userData);

            if (courseId != course.CourseId)
            {
                return BadRequest("Ids do not match");
            }
            // TODO logging
            var updatedCourse = service.UpdateCourse(course);

            var link = Url.Route("DefaultApi", new { controller = "Courses", courseId = updatedCourse.CourseId });

            logger.Info("Course {@courseId} created at route {@route}", updatedCourse.CourseId, link);

            // TODO ERROR HANDLING
            return CreatedAtRoute("DefaultApi", new { controller = "Courses", courseId = updatedCourse.CourseId }, updatedCourse);
        }

        [Route("{courseId}")]
        [HttpDelete]
        public IHttpActionResult DeleteCourse(int courseId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Change Course by {@userData}",userData);

            // TODO logging
            var deletedCourse = service.DeleteCourse(courseId);

            if (deletedCourse == null)
            {
                return NotFound();
            }


            // TODO ERROR HANDLING
            return Ok(deletedCourse);
        }


        #region Teachings
        [Route("{courseId}/teachers")]
        [Authorize(Roles = "admins")]
        [HttpGet]
        public IHttpActionResult GetTeachingAssignments(int courseId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Teachings for Course {@courseId} by {@userData}", courseId, userData);

            return Ok(service.GetAllTeachings(courseId));
        }

        [Route("{courseId}/teachers/{teacherId}")]
        [HttpGet]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetTeachingAssignments(int courseId, int teacherId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Teaching for Course {@courseId} and Teacher {@teacherId} by {@userData}", courseId, teacherId, userData);

            return Ok(service.GetTeaching(courseId, teacherId));
        }

        [Route("{courseId}/teachers")]
        [HttpPost]
        [Authorize(Roles = "admins")]
        public IHttpActionResult CreateTeachingAssignment(int courseId, TeachingDto teaching)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create Teaching {@teachingData} for Course {@courseId} by {@userData}", teaching, courseId, userData);

            return Ok(service.CreateTeachingAssignment(teaching));
        }
        #endregion

        #region Programs
        [Route("{courseId}/teachers/{teacherId}/classrooms")]
        [HttpGet]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetAllClassRoomsPrograms(int courseId, int teacherId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Programs for Course {@courseId} and Teacher {@teacherId} by {@userData}", courseId, teacherId, userData);

            return Ok(service.GetAllPrograms(courseId, teacherId));
        }

        [Route("{courseId}/teachers/{teacherId}/classrooms/{classRoomId}")]
        [HttpGet]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetClassRoomProgram(int courseId, int teacherId, int classRoomId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Program for Course {@courseId} Teacher {@teacherId} and ClassRoom {@classRoomId} by {@userData}", courseId, teacherId, classRoomId, userData);

            return Ok(service.GetProgram(courseId, teacherId, classRoomId));
        }

        [Route("{courseId}/teachers/{teacherId}/classrooms")]
        [HttpPost]
        [Authorize(Roles = "admins")]
        public IHttpActionResult CreateClassRoomProgram(ProgramDto dto)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create Program {@programData} by {@userData}", dto, userData);

            ProgramDto createdProgram = service.CreateProgram(dto);

            var link = Url.Route("DefaultApi", new
            {
                controller = "GetClassRoomProgram",
                courseId = createdProgram.CourseId,
                teacherId = createdProgram.TeacherId,
                classRoomId = createdProgram.ClassRoomId
            });

            Uri.TryCreate(link, UriKind.Absolute, out Uri uri);

            logger.Info("Program {@programId} created at route {@route}", createdProgram.ClassRoomId, link);

            return Created(uri, createdProgram); 
        }
        #endregion

        #region Takings
        [Route("{courseId}/teachers/{teacherId}/classrooms/{classRoomId}/students")]
        [HttpGet]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetStudentsTakings(int courseId, int teacherId, int classRoomId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Students taking Course {@courseId} with Teacher {@teacherId} in Classroom {@classRoomId} by {@userData}", courseId, teacherId, classRoomId, userData);

            return Ok(service.GetAllTakings(courseId, teacherId, classRoomId));
        }

        [Route("{courseId}/teachers/{teacherId}/classrooms/{classRoomId}/students/{studentId}")]
        [HttpGet]
        [Authorize(Roles = "admins")]
        public IHttpActionResult GetStudentTaking(int courseId, int teacherId, int classRoomId, int studentId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Student {@studentId} taking Course {@courseId} with Teacher {@teacherId} in Classroom {@classRoomId} by {@userData}", studentId, courseId, teacherId, classRoomId, userData);

            return Ok(service.GetTaking(courseId, teacherId, classRoomId, studentId));
        }

        [Route("{courseId}/teachers/{teacherId}/classrooms/{classRoomId}/students")]
        [HttpPost]
        [Authorize(Roles = "admins")]
        public IHttpActionResult CreateStudentTaking(TakingDto taking)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Create Taking {@takingData} by {@userData}", taking, userData);

            return Ok(service.CreateTaking(taking));
        }
        #endregion

        #region QUERY
        /// <summary>
        /// Get courses based on query
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
        public IHttpActionResult GetCoursesByQuery(
            [FromUri]int? teacherId = null,
            [FromUri]int? studentId = null,
            [FromUri]int? parentId = null,
            [FromUri]int? courseId = null,
            [FromUri]int? classRoomId = null,
            [FromUri]int? schoolGrade = null)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Teachers by {@userData}", userData);

            var userInfo = Utilities.WebApi.IdentityHelper.FetchUserData(RequestContext);

            if (userInfo.UserId == null)
            {
                return Unauthorized();
            }

            var userId = userInfo.UserId ?? 0;

            if (userInfo.IsAdmin)
            {
                var results = service.GetCoursesByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = service.GetCoursesByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = service.GetCoursesByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = service.GetCoursesByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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
        #endregion
    }
}
