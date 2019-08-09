using eGradeBook.Models.Dtos.Admins;
using eGradeBook.Models.Dtos.Logging;
using eGradeBook.Services;
using eGradeBook.Utilities.WebApi;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace eGradeBook.Controllers
{
    /// <summary>
    /// Web api controller for working with Admins and for admin exclusive functionalities, like accessing log files, for example.
    /// </summary>
    [RoutePrefix("api/admins")]
    public class AdminsController : ApiController
    {
        private IAdminsService service;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service"></param>
        public AdminsController(IAdminsService service)
        {
            this.service = service;
        }

        /// <summary>
        /// First version of working with log files. Needs to be deleted or at least renamed
        /// </summary>
        /// <returns></returns>
        // logging probably also has to be done through services...
        [Route("logging-structure")]
        public IHttpActionResult GetLogfiles()
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get log files by {@userData}", userData);

            string logsFolder = HttpContext.Current.Server.MapPath("~/logs");

            var files = Directory.EnumerateFiles(logsFolder, "*.log");
            var fullFiles = Directory.GetFiles(logsFolder, "*.log");

            var dirInfo = new DirectoryInfo(logsFolder);

            var fileInfoCollection = dirInfo.GetFiles("*.log"); 

            foreach (var finfo in fileInfoCollection)
            {
                // can access any property you need
            }

            // either download one-by-one, or in groups or everything ?

            // api/admin/logs/download-all
            // api/admin/logs -> for listing, getting filenames
            // api/admin/logs/download-selected
            // api/admin/logs/{logfileName:string} -> download one. 

            // a few more possibilites .. download by level, download by functionality (services, repos, api access etc)

            return Ok();
        }


        /// <summary>
        /// Returns a list of accessible log files
        /// </summary>
        /// <returns>A Json object of type LogsDto</returns>
        [Route("logs")]
        [HttpGet]
        public IHttpActionResult GetListOfLogfiles()
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get list of log files by {@userData}", userData);

            LogsDto logsDto = new LogsDto();

            string logsFolder = HttpContext.Current.Server.MapPath("~/logs");

            var files = Directory.EnumerateFiles(logsFolder, "*.log");
            var fullFiles = Directory.GetFiles(logsFolder, "*.log");

            var dirInfo = new DirectoryInfo(logsFolder);

            var fileInfoCollection = dirInfo.GetFiles("*.log");

            foreach (var finfo in fileInfoCollection)
            {
                logsDto.Logs.Add(new LogsDto.LogDto()
                {
                    FileName = finfo.Name,
                    Path = finfo.FullName,
                    LastModified = finfo.LastWriteTime.ToShortDateString(),
                    Size = finfo.Length.ToString()
                });
            }

            return Ok(logsDto);
        }
        
        /// <summary>
        /// Open a log file for reading, as a text/plain resource
        /// </summary>
        /// <param name="logfile"></param>
        /// <returns></returns>
        [Route("logs/{logfile}")]
        [HttpGet]
        public HttpResponseMessage GetLogByFileName(string logfile)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get log file {@logFile} by {@userData}", logfile, userData);

            string logsFolder = HttpContext.Current.Server.MapPath("~/logs");

            var dirInfo = new DirectoryInfo(logsFolder);

            var files = dirInfo.GetFiles($"{logfile}.log");

            if (files.Count() != 1)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var path = files[0].FullName;

            string data;

            using (var reader = new StreamReader(path))
            {
                data = reader.ReadToEnd();
            }


            var response = new HttpResponseMessage(HttpStatusCode.OK);
            var message = new StringContent(data, encoding: Encoding.UTF8, mediaType: "text/html");
            response.Content = message;

            return response;           
        }

        /// <summary>
        /// Get an admin user by Id
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{adminId}", Name = "GetAdminById")]
        [ResponseType(typeof(AdminDto))]
        public IHttpActionResult GetAdminById(int adminId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Get Admin {@adminId} by {@userData}", adminId, userData);

            return Ok(service.GetAdminById(adminId));
        }

        /// <summary>
        /// Get all Admin users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        [ResponseType(typeof(IEnumerable<AdminDto>))]
        public IHttpActionResult GetAdmins()
        {
            return Ok(service.GetAllAdmins());
        }


        #region Update & Delete

        /// <summary>
        /// Update personal info for an admin user
        /// </summary>
        /// <param name="adminId"></param>
        /// <param name="admin"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{adminId}")]
        [ResponseType(typeof(AdminDto))]
        public IHttpActionResult PutUpdateAdmin(int adminId, AdminDto admin)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Update Admin {@adminId} by {@userData}", adminId, userData);

            return Ok(service.UpdateAdmin(adminId, admin));
        }

        /// <summary>
        /// Delete an admin user
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{adminId}")]
        [ResponseType(typeof(AdminDto))]
        public IHttpActionResult DeleteAdmin(int adminId)
        {
            var userData = IdentityHelper.GetLoggedInUser(RequestContext);

            logger.Info("Delete Admin {@adminId} by {@userData}", adminId, userData);

            return Ok(service.DeleteAdmin(adminId));
        }

        #endregion

        #region Reports
        // Number of courses, link to courses
        // Number of teacher, link to teachers
        // Teachers not teaching any course, number, link to each
        // Courses not beign taught by any teacher, number, link to each
        // Number of classrooms, link to classrooms
        // Classrooms with no program
        // Courses not in a program --> through teaching not in a program or directly?
        // Number of students, link to students
        // Students not enrolled in a classroom, link to them
        // Number of students by classroom
        // Students enrolled in a classroom not taking any offered course
        // For students in a classroom : courses offererd, courses taken
        // Students without parents, parents without children
        // (1) IMPLEMENT FIRST: Teachings without program.


        #endregion
    }
}

