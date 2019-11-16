using eGradeBook.Models.Dtos.Programs;
using eGradeBook.Services;
using eGradeBook.SwaggerHelpers.Examples;
using eGradeBook.Utilities.WebApi;
using NLog;
using Swashbuckle.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Temporary Api for programs
    /// NOTE this will not remain in the final api!
    /// NOTE final grades will also only be an aspect of either takings or grades... but i'm not completely convinced yet of that
    /// </summary>
    [RoutePrefix("api/programs")]
    [Authorize]
    public class ProgramsController : ApiController
    {
        private IProgramsService programs;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="programs"></param>
        public ProgramsController(IProgramsService programs)
        {
            this.programs = programs;
        }

        /// <summary>
        /// Create new program
        /// </summary>
        /// <param name="programDto"></param>
        /// <returns></returns>
        [Route("")]
        [HttpPost]
        [SwaggerRequestExample(typeof(ProgramDto), typeof(CreateProgramExample))]
        public IHttpActionResult PostCreateProgram(ProgramDto programDto)
        {
            logger.Info("Create program {@programData}", programDto);

            var createdProgram = programs.CreateProgram(programDto);

            return CreatedAtRoute("GetProgram", new { programId = createdProgram.ProgramId }, createdProgram);
        }

        /// <summary>
        /// Get program by Id
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        [Route("{programId}", Name = "GetProgram")]
        [HttpGet]
        [ResponseType(typeof(ProgramDto))]
        public IHttpActionResult GetProgram(int programId)
        {
            logger.Info("Get program {@programId}", programId);

            return Ok(programs.GetProgramById(programId));
        }

        [Route("")]
        [HttpGet]
        public IHttpActionResult GetAllPrograms()
        {
            logger.Info("Get all programs");

            return Ok(programs.GetAllPrograms());
        }

        [Route("{programId}")]
        [HttpPut]
        public IHttpActionResult UpdateProgram(int programId, ProgramDto programDto)
        {
            logger.Info("Update program {@programId} {@programData}", programId, programDto);

            if (programId != programDto.ProgramId)
            {
                return BadRequest("Id mismatch");
            }

            ProgramDto updatedProgram = programs.UpdateProgram(programDto);

            return Ok(updatedProgram);
        }


        #region QUERY
        /// <summary>
        /// Get programs based on query
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
        public IHttpActionResult GetProgramsByQuery(
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
                var results = programs.GetProgramsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = programs.GetProgramsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = programs.GetProgramsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

                var results = programs.GetProgramsByQuery(teacherId, studentId, parentId, courseId, classRoomId, schoolGrade);

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

        [Route("{programId}")]
        [HttpDelete]
        public IHttpActionResult DeleteProgramById(int programId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Trace("Delete Teaching {@programId} by {@userData}", programId, userData);

            var result = programs.DeleteProgram(programId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
