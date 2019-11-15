using eGradeBook.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace eGradeBook.Controllers
{
    public class FilesController : ApiController
    {

        private IFileResourcesService fileResourcesService;
        private IUsersService usersService;

        public FilesController(IUsersService usersService, IFileResourcesService fileResourcesService)
        {
            this.usersService = usersService;
            this.fileResourcesService = fileResourcesService;
        }


        // GET api/files/{id}
        [Route("api/files/{id}")]
        public IHttpActionResult GetFileResource(int id)
        {
            return Ok(fileResourcesService.GetFileResource(id));
        }

        // GET api/files/{id}/data
        [Route("api/files/{id}/data")]
        public IHttpActionResult GetFileResourceData(int id)
        {
            // 1. Initialize the response
            HttpResponseMessage httpResponse = Request.CreateResponse(); // new HttpResponseMessage();

            // 2. Check if resource exists
            var fileResource = fileResourcesService.GetFileResource(id);

            if (fileResource == null)
            {
                httpResponse.StatusCode = HttpStatusCode.NotFound;
                return ResponseMessage(httpResponse);
            }

            // 3. Build up the response
            // 3.1 It will be of type Multipart Form Data
            httpResponse.StatusCode = HttpStatusCode.OK;
            var responseContent = new MultipartFormDataContent();

            // 3.2 Create the 'part'
            var file = new StreamContent(fileResourcesService.RetrieveDataAsFileStream(fileResource.Id));
            file.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            file.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "tiny.png",
                Name = "tiny.png"
            };

            // 3.3 Add the 'part' to the multipart http content
            responseContent.Add(file);

            // 4 Add multipart content to response
            httpResponse.Content = responseContent;

            // 4. Serve back the file to the user
            return ResponseMessage(httpResponse);
        }

        // GET api/files/{id}/data
        [Route("api/files/{id}/get")]
        public IHttpActionResult GetFileResourceDataAsFile(int id)
        {
            // 1. Initialize the response
            HttpResponseMessage httpResponse = Request.CreateResponse(); // new HttpResponseMessage();

            // 2. Check if resource exists
            var fileResource = fileResourcesService.GetFileResource(id);

            if (fileResource == null)
            {
                httpResponse.StatusCode = HttpStatusCode.NotFound;
                return ResponseMessage(httpResponse);
            }

            // 3. Build up the response
            httpResponse.StatusCode = HttpStatusCode.OK;

            var fileStream = fileResourcesService.RetrieveDataAsFileStream(fileResource.Id);
            var streamContent = new StreamContent(fileStream);

            // "image/png", "image/jpg", "image/gif", "application/octet-stream"
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
            {
                FileName = fileResource.Description
            };

            httpResponse.Content = streamContent;

            return ResponseMessage(httpResponse);
        }    
    }
}
