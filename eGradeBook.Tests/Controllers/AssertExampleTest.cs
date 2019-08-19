using Microsoft.VisualStudio.TestTools.UnitTesting;
using eGradeBook.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eGradeBook.Services;
using eGradeBook.Models.Dtos.Accounts;
using eGradeBook.Models.Dtos.Admins;
using eGradeBook.Models.Dtos.Parents;
using eGradeBook.Models.Dtos.Students;
using eGradeBook.Models.Dtos.Teachers;
using eGradeBook.Models;
using eGradeBook.Models.Dtos.ClassRooms;
using System.Security.Claims;
using System.Threading;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.Owin.Testing;

namespace eGradeBook.Tests.Controllers
{
    [TestClass]
    public class AssertExampleTest
    {
        [TestMethod]
        public void ControllerShouldBeCreated()
        {
            var controller = new ClassRoomsController(new FakeClassService());
            // controller.Request = new System.Net.Http.HttpRequestMessage();


            // according to https://stackoverflow.com/questions/19936892/how-do-i-unit-test-web-api-action-method-when-it-returns-ihttpactionresult
            //controller.Request = new HttpRequestMessage() {
            //    Properties = {
            //        {
            //            HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration()
            //        }
            //    }
            //};
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();


            controller.RequestContext = new System.Web.Http.Controllers.HttpRequestContext();


            var c1 = new Claim(ClaimTypes.Name, "WebApiUser");
            var c2 = new Claim(ClaimTypes.Role, "User");
            var c3 = new Claim(ClaimTypes.Role, "PowerUser");

            var principal = new ClaimsPrincipal();
            var identity = new ClaimsIdentity();
            identity.AddClaim(c1);
            identity.AddClaim(c2);
            identity.AddClaim(c3);

            principal.AddIdentity(identity);
            var server = TestServer.Create<Startup>();
            controller.RequestContext.Principal = principal;
            
            var result = controller.GetClassRoomById(2);
            var contentResult = result as OkNegotiatedContentResult<ClassRoomDto>;

            var theDto = contentResult.Content;
            // Sync
            //var someResult = result.ExecuteAsync(CancellationToken.None).Result;
            //var stringTrueResult = someResult.Content.ReadAsStringAsync().Result;

            // Async
            //var trueResult = await result.ExecuteAsync(new System.Threading.CancellationToken());
            //var stringResult = await trueResult.Content.ReadAsStringAsync();
            Assert.AreEqual("5a", theDto.Name);
        }
    }

    public class FakeClassService : IClassRoomsService
    {
        public ClassRoomDto CreateClassRoom(ClassRoomRegistrationDto classRoom)
        {
            throw new NotImplementedException();
        }

        public void CreateClassRoomProgram(ClassRoomProgramDto program)
        {
            throw new NotImplementedException();
        }

        public ClassRoomDto DeleteClassRoom(int classRoomId)
        {
            throw new NotImplementedException();
        }

        public ClassRoomDto EnrollStudent(ClassRoomEnrollStudentDto enroll)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ClassRoomDto> GetAllClassRooms()
        {
            throw new NotImplementedException();
        }

        public ClassRoom GetClassRoom(int classRoomId)
        {
            return new ClassRoom()
            {
                Id = classRoomId,
                Name = "5a",
                ClassGrade = 5
            };
        }

        public ClassRoomDto GetClassRoomById(int classRoomId)
        {
            return new ClassRoomDto()
            {
                ClassRoomId = classRoomId,
                Name = "5a",
                SchoolGrade = 5
            };
        }

        public ClassRoomDto UpdateClassRoom(int classRoomId, ClassRoomDto classRoom)
        {
            throw new NotImplementedException();
        }
    }
}
