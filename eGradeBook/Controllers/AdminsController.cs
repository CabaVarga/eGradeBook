using eGradeBook.Models.Dtos.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    [RoutePrefix("api/admins")]
    public class AdminsController : ApiController
    {
        // logging probably also has to be done through services...
        public IHttpActionResult GetLogfiles()
        {
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

        [Route("logs")]
        [HttpGet]
        public IHttpActionResult GetListOfLogfiles()
        {
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
        
        [Route("logs/{logfile}")]
        [HttpGet]
        public HttpResponseMessage GetLogByFileName(string logfile)
        {
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
    }
}
