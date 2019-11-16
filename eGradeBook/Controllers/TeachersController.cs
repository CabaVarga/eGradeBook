using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Services;
using eGradeBook.Utilities.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/teachers")]
    [Authorize]
    public class TeachersController : ApiController
    {
        private ITeachersService teachersService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public TeachersController(ITeachersService service)
        {
            this.teachersService = service;
        }

        /// <summary>
        /// Return all teachers in the system.
        /// </summary>
        /// <returns>A Json array of Teacher Dtos</returns>
        [Route("")]
        [ResponseType(typeof(IEnumerable<TeacherDto>))]
        [Authorize(Roles = "admins")]
        [HttpGet]
        public IHttpActionResult GetAllTeachers()
        {
            var user = IdentityHelper.GetLoggedInUser(RequestContext);
            logger.Info("User {@userData} is requesting a list of all teacher", user);

            var teachers = teachersService.GetAllTeachers();

            return Ok(teachers);
        }

        /// <summary>
        /// Retrieve a teacher by its Id.
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns>A Json object of the Teacher Dto</returns>
        [Route("{teacherId:int}", Name = "GetTeacherById")]
        [ResponseType(typeof(TeacherDto))]
        [HttpGet]
        public IHttpActionResult GetTeacherById(int teacherId)
        {
            var user = IdentityHelper.GetLoggedInUser(RequestContext);
            logger.Info("User {@userData} is requesting a teacher by Id {teacherId}", user, teacherId);

            var teacher = teachersService.GetTeacherById(teacherId);

            if (teacher == null)
            {
                return NotFound();
            }

            return Ok(teacher);
        }


        // TODO Add the rest of CRUD methods. We have create (in account), read (all and one), we need update and delete.
        // Watch what will happen in UserRole if I delete a subject... Can I even delete without clearing that first?
        // Should I even bother with changes here or every change should be done through Identity / UserService?

        // Let's add them here. In case of the other approach i will refactor.

        /// <summary>
        /// Update personal data for given teacher.
        /// NOTE this will go to account!
        /// </summary>
        /// <param name="teacherId">Id of teacher</param>
        /// <param name="teacher">Json object of type Teacher Dto</param>
        /// <returns>A Json object of the Teacher Dto with the updated personal details</returns>
        [HttpPut]
        [Route("{teacherId}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(TeacherDto))]
        public IHttpActionResult PutUpdateTeacher(int teacherId, TeacherDto teacher)
        {
            // check model

            // check id

            // call service

            // return updated teacher
            return Ok(teachersService.UpdateTeacher(teacherId, teacher));
            // catch exception: here or at some central place?
        }

        /// <summary>
        /// Delete a teacher with a given Id
        /// NOTE this will go to account!
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns>A Json object of type Teacher Dto for the deleted teacher</returns>
        [HttpDelete]
        [Route("{teacherId}")]
        [Authorize(Roles = "admins")]
        [ResponseType(typeof(TeacherDto))]
        public async Task<IHttpActionResult> DeleteTeacher(int teacherId)
        {
            // this is from accounts
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Trace("Delete User {@teacherId} by {@userData}", teacherId, userData);

            var result = await teachersService.DeleteTeacher(teacherId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        #region CRUD

        #endregion

        #region QUERY
        /// <summary>
        /// Get teachers based on query
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
        public IHttpActionResult GetTeachersByQuery(
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
                var results = teachersService.GetTeachersByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = teachersService.GetTeachersByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = teachersService.GetTeachersByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = teachersService.GetTeachersByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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